namespace SchemaExtractor.Entities
{
    public class ForeignKeyLink
    {
        public string LeftConstraintName { get; set; }
        public string RightConstraintName { get; set; }
        public string RightRelationName { get; set; }
        public string RightIndexName { get; set; }
        public string UpdateRule { get; set; }
        public string DeleteRule { get; set; }

        public string FormatRightRelationName()
        {
            return (RightRelationName ?? string.Empty).Trim();
        }
        public string FormatUpdateRule()
        {
            return (UpdateRule ?? string.Empty).Trim();
        }
        public string FormatDeleteRule()
        {
            return (DeleteRule ?? string.Empty).Trim();
        }
        public string FormatRightIndexName()
        {
            return (RightIndexName ?? string.Empty).Trim();
        }

        public bool IsExplicitUpdateRule()
        {
            return UpdateRule != null && UpdateRule.Trim() != "RESTRICT";
        }
        public bool IsExplicitDeleteRule()
        {
            return DeleteRule != null && DeleteRule.Trim() != "RESTRICT";
        }
    }
}
