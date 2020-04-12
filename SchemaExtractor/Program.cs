using System.Configuration;
using System.IO;
using FirebirdSql.Data.FirebirdClient;
using SchemaExtractor.Commands;

namespace SchemaExtractor
{
    internal static class Program
    {
        private static void Main()
        {
            var csb = new FbConnectionStringBuilder
            {
                Database = @"localhost:C:\CCare\Data\DEMO.FDB",
                UserID = "sysdba",
                Password = "b4u2l8",
                Port = 3051
            };

            using (var context = new FirebirdSystemContext(csb.ToString()))
            {
                //context.Database.Log = Console.Write;

                var outputDirectory = ConfigurationManager.AppSettings.Get("OutputPath");

                if (!Directory.Exists(outputDirectory))
                    Directory.CreateDirectory(outputDirectory);

                new UpdateTriggerFolder(context).Execute();
                new UpdateProcedureFolder(context).Execute();
                new UpdateViewFolder(context).Execute();
                new UpdateTableFolder(context).Execute();
            }
        }
    }
}
