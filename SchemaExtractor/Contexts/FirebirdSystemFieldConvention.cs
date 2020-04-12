using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SchemaExtractor.Contexts
{
    internal class FirebirdSystemFieldConvention : IStoreModelConvention<EdmProperty>
    {
        public void Apply(EdmProperty property, DbModel model)
        {
            if (property.Name.StartsWith("Rdb"))
            {
                property.Name = SplitCamelCase(property.Name)
                    .ToUpperInvariant()
                    .Replace("RDB_", "RDB$")
                    .TrimStart('_');
            }
        }

        private static string SplitCamelCase(string input)
        {
            return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", "_$1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }
    }
}
