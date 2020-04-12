using System;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace SchemaExtractor.Entities
{
    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class Relation
    {
        //public byte[] RdbViewBlr { get; set; }
        public string RdbViewSource { get; set; }
        //public string RdbDescription { get; set; }
        //public int? RdbRelationId { get; set; }
        public RelationSystemFlag? RdbSystemFlag { get; set; }
        //public int? RdbDbkeyLength { get; set; }
        //public int? RdbFormat { get; set; }
        //public int? RdbFieldId { get; set; }
        public string RdbRelationName { get; set; }
        //public string RdbSecurityClass { get; set; }
        public string RdbExternalFile { get; set; }
        //public int? RdbRuntime { get; set; }
        //public int? RdbExternalDescription { get; set; }
        public string RdbOwnerName { get; set; }
        //public string RdbDefaultClass { get; set; }
        //public int? RdbFlags { get; set; }
        public RelationType? RdbRelationType { get; set; }

        public ICollection<RelationField> RdbRelationFields { get; set; }
        public ICollection<Trigger> RdbTriggers { get; set; }
        public ICollection<RelationConstraint> RdbRelationConstraint { get; set; }
        public ICollection<Index> RdbIndices { get; set; }
        public string FormatName()
        {
            return (RdbRelationName ?? string.Empty).Trim();
        }
        public string FormatTypeName()
        {
            switch (RdbRelationType)
            {
                case RelationType.Persistent:
                    return "Table";
                case RelationType.View:
                    return "View";
                case RelationType.External:
                    return "External";
                case RelationType.Virtual:
                    return "Tables";
                case RelationType.GlobalTempDelete:
                case RelationType.GlobalTempPreserve:
                    return "Global Temporary Table";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum RelationType
    {
        Persistent = 0,
        View = 1,
        External = 2,
        Virtual = 3,
        GlobalTempPreserve = 4,
        GlobalTempDelete = 5
    };

    public enum RelationSystemFlag
    {
        User = 0,
        System = 1,
        Qli = 2,
        CheckConstraint = 3,
        ReferentialConstraint = 4,
        ViewCheck = 5
    };
}
