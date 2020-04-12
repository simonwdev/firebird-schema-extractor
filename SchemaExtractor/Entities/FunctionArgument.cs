using JetBrains.Annotations;

namespace SchemaExtractor.Entities
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class FunctionArgument
    {
        public string RdbFunctionName { get; set; }
        public int? RdbArgumentPosition { get; set; }
        //public int? RdbMechanism { get; set; }
        //public int? RdbFieldType { get; set; }
        //public int? RdbFieldScale { get; set; }
        //public int? RdbFieldLength { get; set; }
        //public int? RdbFieldSubType { get; set; }
        //public int? RdbCharacterSetId { get; set; }
        //public int? RdbFieldPrecision { get; set; }
        //public int? RdbCharacterLength { get; set; }

        public virtual Function RdbFunction { get; set; }
    }
}
