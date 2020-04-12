namespace SchemaExtractor.ReferenceTypes
{
    public enum BlobFieldSubType
    {
        Unknown = -1,
        Binary = 0,
        Text = 1,
        Blr = 2,
        AccessControlList = 3,
        Ranges = 4,
        Summary = 5,
        Format = 6,
        TransactionDescription = 7,
        ExternalFileDescription = 8
    }

    public static class BlobFieldSubTypeExtension
    {
        public static string FormatName(this BlobFieldSubType source)
        {
            switch (source)
            {
                case BlobFieldSubType.Binary:
                    return "BINARY";
                case BlobFieldSubType.Text:
                    return "TEXT";
                case BlobFieldSubType.Blr:
                    return "BLR";
                case BlobFieldSubType.AccessControlList:
                    return "ACL";
                case BlobFieldSubType.Ranges:
                    return "RANGES";
                case BlobFieldSubType.Summary:
                    return "SUMMARY";
                case BlobFieldSubType.Format:
                    return "FORMAT";
                case BlobFieldSubType.TransactionDescription:
                    return "TRANSACTION_DESCRIPTION";
                case BlobFieldSubType.ExternalFileDescription:
                    return "EXTERNAL_FILE_DESCRIPTION";
                default:
                    return "#MISSING";
            }
        }
    }
}
