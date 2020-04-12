using JetBrains.Annotations;

namespace SchemaExtractor.Entities
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class CharacterSet
    {
        public string RdbCharacterSetName { get; set; }
        //public string RdbFormOfUse { get; set; }
        //public int? RdbNumberOfCharacters { get; set; }
        //public string RdbDefaultCollateName { get; set; }
        public int? RdbCharacterSetId { get; set; }
        //public int? RdbSystemFlag { get; set; }
        //public string RdbDescription { get; set; }
        //public string RdbFunctionName { get; set; }
        //public int? RdbBytesPerCharacter { get; set; }

        public string FormatName()
        {
            return (RdbCharacterSetName ?? string.Empty).Trim();
        }
    }
}
