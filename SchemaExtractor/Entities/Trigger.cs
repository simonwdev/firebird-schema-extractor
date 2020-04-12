using JetBrains.Annotations;
using SchemaExtractor.ReferenceTypes;

namespace SchemaExtractor.Entities
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class Trigger
    {
        public string RdbTriggerName { get; set; }
        public string RdbRelationName { get; set; }
        public int? RdbTriggerSequence { get; set; }
        public TriggerType? RdbTriggerType { get; set; }
        public string RdbTriggerSource { get; set; }
        //public int? RdbTriggerBlr {get; set;}
        //public string RdbDescription { get; set; }
        public bool? RdbTriggerInactive { get; set; }
        public int? RdbSystemFlag { get; set; }
        //public int? RdbFlags { get; set; }
        //public int? RdbValidBlr { get; set; }
        //public byte[] RdbDebugInfo {get; set;}

        public virtual Relation RdbRelation { get; set; }

        public string FormatName()
        {
            return (RdbTriggerName ?? string.Empty).Trim();
        }
        public string FormatRelationName()
        {
            return (RdbRelationName ?? string.Empty).Trim();
        }
    }
}
