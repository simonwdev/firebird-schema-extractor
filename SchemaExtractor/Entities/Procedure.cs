using System.Collections.Generic;
using JetBrains.Annotations;

namespace SchemaExtractor.Entities
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class Procedure
    {
        public string RdbProcedureName { get; set; }
        //public int? RdbProcedureId { get; set; }
        //public int? RdbProcedureInputs { get; set; }
        //public int? RdbProcedureOutputs { get; set; }
        //public string RdbDescription { get; set; }
        public string RdbProcedureSource { get; set; }
        ////public byte[] RdbProcedureBlr { get; set; }
        //public string RdbSecurityClass { get; set; }
        //public string RdbOwnerName { get; set; }
        ////public byte[] RdbRuntime { get; set; }
        public int? RdbSystemFlag { get; set; }
        //public int? RdbProcedureType { get; set; }
        ////public int? RdbValidBlr { get; set; }
        ////public byte[] RdbDebugInfo { get; set; }

        public ICollection<ProcedureParameter> RdbProcedureParameters { get; set; }

        public string FormatName()
        {
            return (RdbProcedureName ?? string.Empty).Trim();
        }
    }
}
