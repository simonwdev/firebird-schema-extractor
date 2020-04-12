using SchemaExtractor.Extensions;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using SchemaExtractor.Contexts;
using SchemaExtractor.ReferenceTypes;
using SchemaExtractor.Entities;
using SchemaExtractor.Writers;

namespace SchemaExtractor.Commands
{
    public class UpdateTriggerFolder
    {
        private readonly FirebirdSystemContext _context;

        public UpdateTriggerFolder(FirebirdSystemContext context)
        {
            _context = context;
        }

        public void Execute()
        {
            var outputDirectory = ConfigurationManager.AppSettings.Get("OutputPath");
            var triggerPath = Path.Combine(outputDirectory, "Triggers");

            var triggers = _context.RdbTriggers
                .WhereTypeIsDatabase()
                ;//.Where(a => a.RdbProcedureName == "PAT_MERGE");

            if (!Directory.Exists(triggerPath))
                Directory.CreateDirectory(triggerPath);

            var filesOnDisk = Directory
                .GetFiles(triggerPath)
                .ToHashSet(a => a, StringComparer.InvariantCultureIgnoreCase);

            foreach (var trigger in triggers)
            {
                var procedureName = trigger.FormatName();
                var fileName = Path.Combine(triggerPath, $"{procedureName}.sql");

                filesOnDisk.Remove(fileName);

                using (var writer = File.CreateText(fileName))
                {
                    var nameParameter = new FbParameter("@DEPENDENT_NAME", procedureName);
                    var loadedDependencies = _context.Database.SqlQuery<Dependency>(Resources.SqlQueries.SyntheticKeyRdbDependency_EQUALS, nameParameter)
                        .ToArray();

                    new DependencyWriter(writer).WriteSection(loadedDependencies, procedureName, "Procedure");

                    new RelationTriggerWriter(writer)
                        .WriteTrigger(trigger);

                    writer.WriteLine();
                    writer.WriteLine();

                    var procedurePrivileges = _context.RdbUserPrivileges
                        .Where(a => a.RdbRelationName == procedureName && a.RdbUser != "SYSDBA")
                        .ToArray();

                    new UserPrivilegeWriter(writer).WriteSection(procedurePrivileges, procedureName);
                }
            }

            foreach (var file in filesOnDisk)
            {
                File.Delete(file);
            }
        }
    }
}
