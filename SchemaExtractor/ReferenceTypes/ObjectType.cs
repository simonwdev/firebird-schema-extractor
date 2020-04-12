namespace SchemaExtractor.ReferenceTypes
{
    public enum ObjectType
    {
        Relation = 0,
        View = 1,
        Trigger = 2,
        Computed = 3,
        Validation = 4,
        Procedure = 5,
        ExpressionIndex = 6,
        Exception = 7,
        User = 8,
        Field = 9,
        Index = 10,
        Count = 11,
        UserGroup = 12,
        SqlRole = 13,
        Generator = 14,
        Udf = 15,
        BlobFilter = 16,
        Collation = 17,
    }

    public static class ObjectTypeExtension
    {
        public static string FormatName(this ObjectType? type)
        {
            switch (type)
            {
                case ObjectType.Relation:
                    return "Relation";
                case ObjectType.View:
                    return "View";
                case ObjectType.Trigger:
                    return "Trigger";
                case ObjectType.Computed:
                    return "Computed Column";
                case ObjectType.Validation:
                    return "Validation";
                case ObjectType.Procedure:
                    return "Procedure";
                case ObjectType.ExpressionIndex:
                    return "Expression Index";
                case ObjectType.Exception:
                    return "Exception";
                case ObjectType.User:
                    return "User";
                case ObjectType.Field:
                    return "Field";
                case ObjectType.Index:
                    return "Index";
                case ObjectType.UserGroup:
                    return "User Group";
                case ObjectType.SqlRole:
                    return "Sql Role";
                case ObjectType.Generator:
                    return "Generator";
                case ObjectType.Udf:
                    return "Udf";
                case ObjectType.BlobFilter:
                    return "Blob Filter";
                case ObjectType.Collation:
                    return "Collation";
                default:
                    return "#MISSING#";
            }
        }
    }
}
