using JetBrains.Annotations;

namespace SchemaExtractor.Entities
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class RelationField
    {
        public string RdbFieldName { get; set; }
        public string RdbRelationName { get; set; }
        public string RdbFieldSource { get; set; }
        //public string RdbQueryName { get; set; }
        //public string RdbBaseField { get; set; }
        //public string RdbEditString { get; set; }
        public int? RdbFieldPosition { get; set; }
        //public string RdbQueryHeader { get; set; }
        //public int? RdbUpdateFlag { get; set; }
        //public int? RdbFieldId { get; set; }
        //public int? RdbViewContext { get; set; }
        public string RdbDescription { get; set; }
        ////public int? RdbDefaultValue { get; set; }
        //public int? RdbSystemFlag { get; set; }
        //public string RdbSecurityClass { get; set; }
        //public string RdbComplexName { get; set; }
        public bool? RdbNullFlag { get; set; }
        public string RdbDefaultSource { get; set; }
        public int? RdbCollationId { get; set; }

        public virtual Relation RdbRelation { get; set; }
        public virtual Field RdbField { get; set; }

        public string FormatName()
        {
            return (RdbFieldName ?? string.Empty).Trim();
        }
        public string FormatRelationName()
        {
            return (RdbRelationName ?? string.Empty).Trim();
        }
        public string FormatDescription()
        {
            return (RdbDescription ?? string.Empty).Trim();
        }
    }
}
