using JetBrains.Annotations;
using SchemaExtractor.ReferenceTypes;

namespace SchemaExtractor.Entities
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class UserPrivilege
    {
        public string RdbUser { get; set; }
        public string RdbGrantor { get; set; }
        public string RdbPrivilege { get; set; }
        public int? RdbGrantOption { get; set; }
        public string RdbRelationName { get; set; }
        public string RdbFieldName { get; set; }
        public ObjectType? RdbUserType { get; set; }
        public int? RdbObjectType { get; set; }
        
        public string FormatRelationName()
        {
            return (RdbRelationName ?? string.Empty).Trim();
        }
        public string FormatUser()
        {
            return (RdbUser ?? string.Empty).Trim();
        }
        public string FormatPrivilege()
        {
            var typeValue = (RdbPrivilege ?? string.Empty).Trim();

            switch (typeValue)
            {
                case "A":
                    return "ALL";
                case "S":
                    return "SELECT";
                case "I":
                    return "INSERT";
                case "D":
                    return "DELETE";
                case "R":
                    return "REFERENCE";
                case "U":
                    return "UPDATE";
                case "X":
                    return "EXECUTE";
                default:
                    return "#MISSING#";
            }
        }
        public string FormatUserObject()
        {
            string objectName;
            switch (RdbUserType)
            {
                case ObjectType.Relation:
                case ObjectType.View:
                case ObjectType.Trigger:
                case ObjectType.Procedure:
                case ObjectType.SqlRole:
                case ObjectType.User:
                    objectName = FormatUser();
                    break;
                default:
                    objectName = "#MISSING";
                    break;
            }

            switch (RdbUserType)
            {
                case ObjectType.View:
                    return $"VIEW {objectName}";
                case ObjectType.Trigger:
                    return $"TRIGGER {objectName}";
                case ObjectType.Procedure:
                    return $"PROCEDURE {objectName}";
                case ObjectType.User:
                    return objectName == "PUBLIC" ? $"USER {objectName}" : objectName;
                case ObjectType.UserGroup:
                    return $"GROUP {objectName}";
                case ObjectType.SqlRole:
                    return $"ROLE {objectName}";
                default:
                    return $"{objectName}";
            }
        }
    }
}
