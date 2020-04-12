using SchemaExtractor.Extensions;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using SchemaExtractor.Contexts;
using SchemaExtractor.Entities;
using SchemaExtractor.Writers;

namespace SchemaExtractor.Commands
{
    public class UpdateViewFolder
    {
        private readonly FirebirdSystemContext _context;

        public UpdateViewFolder(FirebirdSystemContext context)
        {
            _context = context;
        }

        public void Execute()
        {
            var outputDirectory = ConfigurationManager.AppSettings.Get("OutputPath");
            var viewsPath = Path.Combine(outputDirectory, "Views");

            var views = _context.RdbRelations
                .WithViewDefaults()
                .WhereTypeIsView()
                ;//.Where(a => a.RdbRelationName == "PAT_PRESCRIPTION_VIEW");

            if (!Directory.Exists(viewsPath))
                Directory.CreateDirectory(viewsPath);

            var filesOnDisk = Directory
                .GetFiles(viewsPath)
                .ToHashSet(a => a, StringComparer.InvariantCultureIgnoreCase);

            foreach (var relation in views)
            {
                var viewName = relation.FormatName();
                var viewFileName = Path.Combine(viewsPath, $"{viewName}.sql");
                var viewTriggerFileName = Path.Combine(viewsPath, $"{viewName}-triggers.sql");

                filesOnDisk.Remove(viewFileName);

                using (var writer = File.CreateText(viewFileName))
                {
                    var tableNameParameter = new FbParameter("@DEPENDENT_NAME", viewName);
                    var tableDependencies = _context.Database.SqlQuery<Dependency>(Resources.SqlQueries.SyntheticKeyRdbDependency_EQUALS, tableNameParameter)
                        .ToArray();

                    new DependencyWriter(writer).WriteSection(tableDependencies, viewName, relation.FormatTypeName());

                    new ViewWriter(writer)
                        .Write(relation);

                    var tablePrivileges = _context.RdbUserPrivileges
                        .Where(a => a.RdbRelationName == viewName && a.RdbUser != "SYSDBA")
                        .ToArray();

                    writer.WriteLine();

                    new UserPrivilegeWriter(writer).WriteSection(tablePrivileges, viewName);

                    writer.WriteLine();
                    writer.WriteLine();
                }

                var triggers = relation.RdbTriggers
                    .OrderBy(a => a.RdbTriggerType)
                    .ThenBy(a => a.RdbTriggerSequence)
                    .ThenBy(a => a.RdbTriggerName)
                    .ToArray();

                if (triggers.Any())
                {
                    filesOnDisk.Remove(viewTriggerFileName);

                    using (var writer = File.CreateText(viewTriggerFileName))
                    {
                        writer.WriteLine($"/* Triggers for {viewName} */");
                        writer.WriteLine();

                        foreach (var trigger in triggers)
                        {
                            var triggerName = trigger.FormatName();
                            var triggerNameParameter = new FbParameter("@DEPENDENT_NAME", triggerName);
                            var triggerDependencies = _context.Database
                                .SqlQuery<Dependency>(Resources.SqlQueries.SyntheticKeyRdbDependency_EQUALS,
                                    triggerNameParameter)
                                .ToArray();

                            new DependencyWriter(writer).WriteSection(triggerDependencies, triggerName, "Trigger");

                            new RelationTriggerWriter(writer).WriteTrigger(trigger);

                            if (trigger != triggers.Last())
                            {
                                writer.WriteLine();
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
