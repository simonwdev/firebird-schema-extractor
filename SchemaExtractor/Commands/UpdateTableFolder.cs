using SchemaExtractor.Extensions;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using SchemaExtractor.Contexts;
using SchemaExtractor.Entities;
using SchemaExtractor.Writers;

namespace SchemaExtractor.Commands
{
    public class UpdateTableFolder
    {
        private readonly FirebirdSystemContext _context;

        public UpdateTableFolder(FirebirdSystemContext context)
        {
            _context = context;
        }

        public void Execute()
        {
            var outputDirectory = ConfigurationManager.AppSettings.Get("OutputPath");
            var tablesPath = Path.Combine(outputDirectory, "Tables");

            var characterSets = _context.RdbCharacterSets.ToArray();
            var collations = _context.RdbCollations.ToArray();

            var tables = _context.RdbRelations
                .WithTableDefaults()
                .WhereTypeIsTable()
                ;//.Where(a => a.RdbRelationName == "PAT_PRESCRIPTION");

            if (!Directory.Exists(tablesPath))
                Directory.CreateDirectory(tablesPath);

            var filesOnDisk = Directory
                .GetFiles(tablesPath)
                .ToHashSet(a => a, StringComparer.InvariantCultureIgnoreCase);

            foreach (var relation in tables)
            {
                var tableName = relation.FormatName();
                var tableFileName = Path.Combine(tablesPath, $"{tableName}.sql");
                var tableTriggerFileName = Path.Combine(tablesPath, $"{tableName}-triggers.sql");

                filesOnDisk.Remove(tableFileName);

                using (var writer = File.CreateText(tableFileName))
                {
                    var tableNameParameter = new FbParameter("@DEPENDENT_NAME", tableName);
                    var tableDependencies = _context.Database.SqlQuery<Dependency>(Resources.SqlQueries.SyntheticKeyRdbDependency_EQUALS, tableNameParameter)
                        .ToArray();

                    new DependencyWriter(writer).WriteSection(tableDependencies, tableName, relation.FormatTypeName());

                    new TableWriter(writer, characterSets, collations)
                        .Write(relation);

                    var fkParameter = new FbParameter("@NAME", tableName);
                    var foreignKeyLinks = _context.Database.SqlQuery<ForeignKeyLink>(Resources.SqlQueries.RdbForeignKeyLink, fkParameter)
                        .ToDictionary(a => a.LeftConstraintName);

                    var rightIndexNames = foreignKeyLinks.Values
                        .Select(a => a.FormatRightIndexName())
                        .ToArray();

                    var rightIndexSegmentDictionary = _context.RdbIndexSegments
                        .Where(a => rightIndexNames.Contains(a.RdbIndexName))
                        .GroupBy(a => a.RdbIndexName)
                        .ToDictionary(a => a.Key, a => a.ToArray());

                    foreach (var relationConstraintGroup in relation.RdbRelationConstraint.Where(c => c.RdbIndex != null).GroupBy(a => a.FormatConstraintType()))
                    {
                        writer.WriteLine($"/* {CultureInfo.InstalledUICulture.TextInfo.ToTitleCase(relationConstraintGroup.Key.ToLower())}s */");

                        foreach (var constraint in relationConstraintGroup.OrderBy(a => a.FormatName()))
                        {
                            var leftIndex = constraint.RdbIndex;
                            var leftFieldText = string.Join(", ", leftIndex.RdbIndexSegments.OrderBy(a => a.RdbFieldPosition).Select(a => a.FormatFieldName()));

                            var link = foreignKeyLinks.GetValueOrDefault(constraint.RdbConstraintName);

                            writer.Write($"ALTER TABLE {constraint.FormatRelationName()} ");
                            writer.Write($"ADD CONSTRAINT {constraint.FormatName()} {constraint.FormatConstraintType()} ({leftFieldText}) ");

                            if (link != null)
                            {
                                var rightIndexSegments = rightIndexSegmentDictionary.GetValueOrDefault(link.RightIndexName);

                                if (rightIndexSegments != null)
                                {
                                    var rightFieldText = string.Join(", ", rightIndexSegments.OrderBy(a => a.RdbFieldPosition).Select(a => a.FormatFieldName()));

                                    writer.Write($"REFERENCES {link.FormatRightRelationName()} ({rightFieldText})");

                                    if (link.IsExplicitUpdateRule())
                                        writer.Write($" ON UPDATE {link.FormatUpdateRule()}");

                                    if (link.IsExplicitDeleteRule())
                                        writer.Write($" ON DELETE {link.FormatDeleteRule()}");
                                }
                                else
                                {
                                    writer.Write("#MISSING#");
                                }
                            }

                            writer.WriteLine();
                        }

                        writer.WriteLine();
                    }

                    var keyIndexSet = relation.RdbRelationConstraint
                        .Select(a => a.RdbIndexName)
                        .ToHashSet(a => a, StringComparer.InvariantCultureIgnoreCase);

                    // NOTE: Exclude key indices.
                    var tableIndices = relation.RdbIndices
                        .Where(a => !keyIndexSet.Contains(a.RdbIndexName))
                        .OrderBy(a => a.RdbIndexName)
                        .ToArray();

                    if (tableIndices.Any())
                    {
                        writer.WriteLine("/* Indices */");

                        foreach (var index in tableIndices)
                        {
                            var uniqueText = (index.RdbUniqueFlag ?? false) ? "UNIQUE " : string.Empty;
                            var directionText = (index.RdbIndexType ?? 0) > 0 ? "DESCENDING " : string.Empty;

                            writer.Write($"CREATE {uniqueText}{directionText}INDEX {index.FormatName()} ON {index.FormatRelationName()}");

                            if (index.RdbIndexSegments.Any())
                            {
                                var fieldText = string.Join(", ", index.RdbIndexSegments.OrderBy(a => a.RdbFieldPosition).Select(a => a.FormatFieldName()));

                                writer.Write($" ({fieldText})");
                            }
                            else
                            {
                                writer.Write($" COMPUTED BY {index.RdbExpressionSource}");
                            }

                            writer.WriteLine();
                        }

                        writer.WriteLine();
                    }
                    
                    var commentFields = relation.RdbRelationFields.Where(c => c.RdbDescription != null)
                        .OrderBy(a => a.FormatName())
                        .ToArray();

                    if (commentFields.Any())
                    {
                        writer.WriteLine($"/* Field Comments */");

                        foreach (var field in commentFields)
                        {
                            writer.WriteLine($"COMMENT ON COLUMN {field.FormatRelationName()}.{field.FormatName()} IS '{field.FormatDescription()}'");
                        }

                        writer.WriteLine();
                    }

                    var tablePrivileges = _context.RdbUserPrivileges
                        .Where(a => a.RdbRelationName == tableName && a.RdbUser != "SYSDBA")
                        .ToArray();

                    new UserPrivilegeWriter(writer).WriteSection(tablePrivileges, tableName);

                    writer.WriteLine();
                    writer.WriteLine();
                }

                var triggers = relation.RdbTriggers
                    .OrderBy(a => a.FormatName())
                    .ToArray();

                if (triggers.Any())
                {
                    filesOnDisk.Remove(tableTriggerFileName);

                    using (var writer = File.CreateText(tableTriggerFileName))
                    {
                        writer.WriteLine($"/* Triggers for {tableName} */");
                        writer.WriteLine();

                        foreach (var trigger in triggers)
                        {
                            var triggerName = trigger.FormatName();
                            var triggerNameParameter = new FbParameter("@DEPENDENT_NAME", triggerName);
                            var triggerDependencies = _context.Database
                                .SqlQuery<Dependency>(Resources.SqlQueries.SyntheticKeyRdbDependency_EQUALS, triggerNameParameter)
                                .ToArray();

                            new DependencyWriter(writer).WriteSection(triggerDependencies, triggerName, "Trigger");

                            new RelationTriggerWriter(writer).WriteTrigger(trigger);

                            if (trigger != triggers.Last())
                            {
                                writer.WriteLine();
                            }
                        }
                    }
                }
            }

            foreach (var file in filesOnDisk)
            {
                File.Delete(file);
            }
        }
    }
}
