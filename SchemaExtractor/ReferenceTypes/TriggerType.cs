using System;

namespace SchemaExtractor.ReferenceTypes
{
    public enum TriggerType
    {
        BeforeInsert = 1,
        AfterInsert = 2,
        BeforeUpdate = 3,
        AfterUpdate = 4,
        BeforeDelete = 5,
        AfterDelete = 6,
        BeforeInsertOrUpdate = 17,
        AfterInsertOrUpdate = 18,
        BeforeInsertOrDelete = 25,
        AfterInsertOrDelete = 26,
        BeforeUpdateOrDelete = 27,
        AfterUpdateOrDelete = 28,
        BeforeInsertOrUpdateOrDelete = 113,
        AfterInsertOrUpdateOrDelete = 114,
        OnConnect = 8192,
        OnDisconnect = 8193,
        OnTransactionStart = 8194,
        OnTransactionCommit = 8195,
        OnTransactionRollback = 8196,
    }

    public static class TriggerTypeExtension
    {
        public static string FormatName(this TriggerType? source)
        {
            switch (source)
            {
                case null:
                    return "#MISSING#";
                case TriggerType.BeforeInsert:
                    return "BEFORE INSERT";
                case TriggerType.AfterInsert:
                    return "AFTER INSERT";
                case TriggerType.BeforeUpdate:
                    return "BEFORE UPDATE";
                case TriggerType.AfterUpdate:
                    return "AFTER UPDATE";
                case TriggerType.BeforeDelete:
                    return "BEFORE DELETE";
                case TriggerType.AfterDelete:
                    return "AFTER DELETE";
                case TriggerType.BeforeInsertOrUpdate:
                    return "BEFORE INSERT OR UPDATE";
                case TriggerType.AfterInsertOrUpdate:
                    return "AFTER INSERT OR UPDATE";
                case TriggerType.BeforeInsertOrDelete:
                    return "BEFORE INSERT OR DELETE";
                case TriggerType.AfterInsertOrDelete:
                    return "AFTER INSERT OR DELETE";
                case TriggerType.BeforeUpdateOrDelete:
                    return "BEFORE UPDATE OR DELETE";
                case TriggerType.AfterUpdateOrDelete:
                    return "AFTER UPDATE OR DELETE";
                case TriggerType.BeforeInsertOrUpdateOrDelete:
                    return "BEFORE INSERT OR UPDATE OR DELETE";
                case TriggerType.AfterInsertOrUpdateOrDelete:
                    return "AFTER INSERT OR UPDATE OR DELETE";
                case TriggerType.OnConnect:
                    return "ON CONNECT";
                case TriggerType.OnDisconnect:
                    return "ON DISCONNECT";
                case TriggerType.OnTransactionStart:
                    return "ON TRANSACTION START";
                case TriggerType.OnTransactionCommit:
                    return "ON TRANSACTION COMMIT";
                case TriggerType.OnTransactionRollback:
                    return "ON TRANSACTION ROLLBACK";
                default:
                    throw new ArgumentOutOfRangeException(nameof(TriggerType), source, null);
            }
        }
    }
}
