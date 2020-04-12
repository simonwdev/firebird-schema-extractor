namespace SchemaExtractor.ReferenceTypes
{
    public enum NumericFieldSubType
    {
        Unknown = 0,
        Numeric = 1,
        Decimal = 2
    }

    public static class IntegralFieldSubTypeExtension
    {
        public static string FormatName(this NumericFieldSubType source)
        {
            switch (source)
            {      
                case NumericFieldSubType.Numeric:
                    return "NUMERIC";
                case NumericFieldSubType.Decimal:
                    return "DECIMAL";
                default:
                    return "#MISSING#";
            }
        }
    }
}
