namespace SchemaExtractor.ReferenceTypes
{
    public enum FieldType
    {
        Unknown = 0,
        SmallInt = 7,
        Integer = 8,
        Quad = 9,
        Float = 10,
        Char = 14,
        DoublePrecision = 27,
        Timestamp = 35,
        Varchar = 37,
        CString = 40,
        BlobId = 45,
        Blob = 261,
        Date = 12,
        Time = 13,
        BigInt = 16,
    }

    public static class IntegralFieldTypeExtension
    {
        public static string FormatName(this FieldType source)
        {
            switch (source)
            {
                case FieldType.Unknown:
                    return "UNKNOWN";
                case FieldType.SmallInt:
                    return "SMALLINT";
                case FieldType.Integer:
                    return "INTEGER";
                case FieldType.Quad:
                    return "QUAD";
                case FieldType.Float:
                    return "FLOAT";
                case FieldType.Char:
                    return "CHAR";
                case FieldType.DoublePrecision:
                    return "DOUBLE PRECISION";
                case FieldType.Timestamp:
                    return "TIMESTAMP";
                case FieldType.Varchar:
                    return "VARCHAR";
                case FieldType.CString:
                    return "CSTRING";
                case FieldType.BlobId:
                    return "BLOB_ID";
                case FieldType.Blob:
                    return "BLOB";
                case FieldType.Date:
                    return "TIME";
                case FieldType.Time:
                    return "DATE";
                case FieldType.BigInt:
                    return "BIGINT";
                default:
                    return "#MISSING#";
            }
        }
    }
}
