using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SchemaExtractor.Entities;
using SchemaExtractor.ReferenceTypes;

namespace SchemaExtractor.Contexts
{
    public static class ProcedureExtensions
    {
        public static IQueryable<Procedure> WithParameters(this IQueryable<Procedure> query)
        {
            return query.Include(procedure => procedure.RdbProcedureParameters.Select(p => p.RdbField));
        }

        public static IQueryable<Procedure> WhereExcludeSystem(this IQueryable<Procedure> query)
        {
            return query.Where(p => p.RdbSystemFlag == null || p.RdbSystemFlag != 1);
        }
    }
    
    public static class RelationExtensions
    {
        public static IQueryable<Relation> WithViewDefaults(this IQueryable<Relation> query)
        {
            return query
                .Include(f => f.RdbRelationFields)
                .Include(a => a.RdbTriggers);
        }

        public static IQueryable<Relation> WithTableDefaults(this IQueryable<Relation> query)
        {
            return query
                .Include(f => f.RdbRelationFields)
                .Include(a => a.RdbTriggers)
                .Include(a => a.RdbIndices.Select(b => b.RdbIndexSegments))
                .Include(a => a.RdbRelationConstraint.Select(b => b.RdbIndex).Select(c => c.RdbIndexSegments));
        }

        public static IQueryable<Relation> WhereTypeIsView(this IQueryable<Relation> query)
        {
            return query.Where(p => p.RdbSystemFlag == RelationSystemFlag.User && p.RdbRelationType == RelationType.View);
        }
        public static IQueryable<Relation> WhereTypeIsTable(this IQueryable<Relation> query)
        {
            return query.Where(p => p.RdbSystemFlag == RelationSystemFlag.User && p.RdbRelationType != RelationType.View && p.RdbViewSource == null);
        }
    }

    public static class TriggerExtensions
    {
        public static IQueryable<Trigger> WhereTypeIsDatabase(this IQueryable<Trigger> query)
        {
            var systemTriggerSet = new HashSet<TriggerType?>
            {
                TriggerType.OnConnect,
                TriggerType.OnDisconnect,
                TriggerType.OnTransactionCommit,
                TriggerType.OnTransactionStart,
                TriggerType.OnTransactionRollback
            };

            return query.Where(a =>
                a.RdbSystemFlag == 0 && a.RdbTriggerType != null && systemTriggerSet.Contains(a.RdbTriggerType));
        }
    }
}
