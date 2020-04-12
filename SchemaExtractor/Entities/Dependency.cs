using JetBrains.Annotations;
using System;
using SchemaExtractor.ReferenceTypes;

namespace SchemaExtractor.Entities
{
    [UsedImplicitly]
    public class Dependency
    {
        //public Guid RdbDependencyId { get; set; }
        public string RdbDependentName { get; set; }
        public string RdbDependedOnName { get; set; }
        public string RdbFieldName { get; set; }
        public ObjectType? RdbDependentType { get; set; }
        public ObjectType? RdbDependedOnType { get; set; }

        public string FormatDependentName()
        {
            return (RdbDependentName ?? string.Empty).Trim();
        }
        public string FormatDependedOnName()
        {
            return (RdbDependedOnName ?? string.Empty).Trim();
        }
    }
}
