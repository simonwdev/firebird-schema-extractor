using System.Collections.Generic;
using JetBrains.Annotations;

namespace SchemaExtractor.Entities
{
    [UsedImplicitly]
    public class Function
    {
        public string RdbFunctionName { get; set; }
        public int? RdbFunctionType { get; set; }
        public string RdbQueryName { get; set; }
        public string RdbDescription { get; set; }
        public string RdbModuleName { get; set; }
        public string RdbEntrypoint { get; set; }
        public int? RdbReturnArgument { get; set; }
        public int? RdbSystemFlag { get; set; }

        public ICollection<FunctionArgument> RdbFunctionArguments { get; set; }

        public string FormatName()
        {
            return (RdbFunctionName ?? string.Empty).Trim();
        }
    }
}
