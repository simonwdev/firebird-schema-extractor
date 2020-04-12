using JetBrains.Annotations;
using SchemaExtractor.ReferenceTypes;

namespace SchemaExtractor.Entities
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class ProcedureParameter
    {
        public string RdbParameterName { get; set; }
        public string RdbProcedureName { get; set; }
        public int RdbParameterNumber { get; set; }
        public ParameterType RdbParameterType { get; set; }
        public string RdbFieldSource { get; set; }
        //public string RdbDescription { get; set; }
        public RelationSystemFlag? RdbSystemFlag { get; set; }
        //public string RdbDefaultValue { get; set; }
        public string RdbDefaultSource { get; set; }
        public int? RdbCollationId { get; set; }
        public bool? RdbNullFlag { get; set; }
        //public int? RdbParameterMechanism { get; set; }
        //public string RdbFieldName { get; set; }
        //public string RdbRelationName { get; set; }

        public virtual Field RdbField { get; set; }

        public string FormatName()
        {
            return (RdbParameterName ?? string.Empty).Trim();
        }

        public bool IsExplicitDomain()
        {
            return FormatName().StartsWith(FirebirdConstants.ImplicitDomainPrefix);
        }
    }

    public enum ParameterType
    {
        Input = 0,
        Output = 1
    }
}
