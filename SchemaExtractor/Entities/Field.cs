using JetBrains.Annotations;
using SchemaExtractor.ReferenceTypes;

namespace SchemaExtractor.Entities
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class Field
    {
        public string RdbFieldName { get; set; }
        //public int RdbQueryName { get; set; }
        //public int RdbValidationBlr { get; set; }
        //public int RdbValidationSource { get; set; }
        public byte[] RdbComputedBlr { get; set; }
        public string RdbComputedSource { get; set; }
        //public int RdbDefaultValue { get; set; }
        public string RdbDefaultSource { get; set; }
        public int RdbFieldLength { get; set; }
        public int RdbFieldScale { get; set; }
        public FieldType? RdbFieldType { get; set; }
        public int? RdbFieldSubType { get; set; }
        //public int RdbMissingValue { get; set; }
        //public int RdbMissingSource { get; set; }
        //public int RdbDescription { get; set; }
        public RelationSystemFlag? RdbSystemFlag { get; set; }
        //public int RdbQueryHeader { get; set; }
        public int? RdbSegmentLength { get; set; }
        //public int RdbEditString { get; set; }
        //public int RdbExternalLength { get; set; }
        //public int RdbExternalScale { get; set; }
        //public int RdbExternalType { get; set; }
        public int? RdbDimensions { get; set; }
        //public int RdbNullFlag { get; set; }
        public int? RdbCharacterLength { get; set; }
        public int? RdbCollationId { get; set; }
        public int? RdbCharacterSetId { get; set; }
        public int? RdbFieldPrecision { get; set; }

        public string FormatName()
        {
            return (RdbFieldName ?? string.Empty).Trim();
        }
        
        public bool IsExplicitDomain()
        {
            return FormatName().StartsWith(FirebirdConstants.ImplicitDomainPrefix);
        }
        public bool IsComputed()
        {
            return RdbComputedBlr != null;
        }

        public NumericFieldSubType SubTypeAsNumeric()
        {
            var value = RdbFieldSubType ?? 0;
            if (value > 2)
                value = 0;

            return (NumericFieldSubType)value;
        }
        public BlobFieldSubType SubTypeAsBlob()
        {
            var value = RdbFieldSubType ?? 0;
            if (value > 8)
                value = (int)BlobFieldSubType.Unknown;

            return (BlobFieldSubType)value;
        }
    }
}
