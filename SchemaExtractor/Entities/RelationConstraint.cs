﻿using JetBrains.Annotations;

namespace SchemaExtractor.Entities
{
    [UsedImplicitly]
    public class RelationConstraint
    {
        /// <Summary>
        /// The Name Of The Table-Level Constraint Defined By The User, Or Otherwise Automatically Generated By the system
        /// </Summary>
        public string RdbConstraintName { get; set; }
        /// <Summary>
        /// The Name Of The Constraint Type: Primary Key, Unique, Foreign Key, Check Or Not Null
        /// </Summary>
        public string RdbConstraintType { get; set; }
        /// <Summary>
        /// The Name Of The Table This Constraint Applies To
        /// </Summary>
        public string RdbRelationName { get; set; }
        /// <Summary>
        /// Currently No In All Cases: Firebird Does Not Yet Support Deferrable Constraints
        /// </Summary>
        public string RdbDeferrable { get; set; }
        /// <Summary>
        /// Currently No In All Cases
        /// </Summary>
        public string RdbInitiallyDeferred { get; set; }
        /// <Summary>
        /// The Name Of The Index That Supports This Constraint. For A Check Or A Not Null Constraint, It Is Null. 
        /// </Summary>
        public string RdbIndexName { get; set; }

        public virtual Relation RdbRelation { get; set; }
        
        public virtual Index RdbIndex { get; set; }

        public string FormatName()
        {
            return (RdbConstraintName ?? string.Empty).Trim();
        }
        public string FormatConstraintType()
        {
            return (RdbConstraintType ?? string.Empty).Trim();
        }
        public string FormatRelationName()
        {
            return (RdbRelationName ?? string.Empty).Trim();
        }

        public bool IsPrimaryKey()
        {
            return FormatConstraintType() == "PRIMARY KEY";
        }
        public bool IsNotNullConstraint()
        {
            return FormatConstraintType() == "NOT NULL";
        }
    }
}