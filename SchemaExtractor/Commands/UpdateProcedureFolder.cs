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
    public class UpdateProcedureFolder
    {
        private readonly FirebirdSystemContext _context;

        public UpdateProcedureFolder(FirebirdSystemContext context)
        {
            _context = context;
        }

        public void Execute()
        {
            var outputDirectory = ConfigurationManager.AppSettings.Get("OutputPath");
            var procedurePath = Path.Combine(outputDirectory, "Procedures");

            var characterSets = _context.RdbCharacterSets.ToArray();

            var procedures = _context.RdbProcedures
                .WithParameters()
                .WhereExcludeSystem()
                ;//.Where(a => a.RdbProcedureName == "PAT_MERGE");

            if (!Directory.Exists(procedurePath))
                Directory.CreateDirectory(procedurePath);

            var filesOnDisk = Directory
                .GetFiles(procedurePath)
                .ToHashSet(a => a, StringComparer.InvariantCultureIgnoreCase);

            foreach (var procedure in procedures)
            {
                var procedureName = procedure.FormatName();
                var fileName = Path.Combine(procedurePath, $"{procedureName}.sql");

                filesOnDisk.Remove(fileName);

                using (var writer = File.CreateText(fileName))
                {
                    var nameParameter = new FbParameter("@DEPENDENT_NAME", procedureName);
                    var loadedDependencies = _context.Database.SqlQuery<Dependency>(Resources.SqlQueries.SyntheticKeyRdbDependency_EQUALS, nameParameter)
                        .ToArray();

                    new DependencyWriter(writer).WriteSection(loadedDependencies, procedureName, "Procedure");

                    new ProcedureWriter(writer, characterSets)
                        .Write(procedure);

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
