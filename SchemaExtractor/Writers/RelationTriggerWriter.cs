using System.IO;
using SchemaExtractor.ReferenceTypes;
using SchemaExtractor.Entities;

namespace SchemaExtractor.Writers
{
    public class RelationTriggerWriter
    {
        private readonly TextWriter _writer;

        public RelationTriggerWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public void WriteTrigger(Trigger trigger)
        {
            var name = trigger.FormatName();
            var activeText = trigger.RdbTriggerInactive ?? false ? "INACTIVE" : "ACTIVE";
            var relationName = trigger.FormatRelationName();

            // NOTE: SQL-2003-compliant syntax for relation triggers 
            
            _writer.WriteLine($"CREATE TRIGGER {name}");
            _writer.WriteLine($"    {activeText}");
            _writer.WriteLine($"    {trigger.RdbTriggerType.FormatName()}");
            _writer.WriteLine($"    POSITION {trigger.RdbTriggerSequence}");

            if (trigger.RdbRelationName != null)
                _writer.WriteLine($"    ON {relationName}");

            if (trigger.RdbTriggerSource != null)
            {
                _writer.WriteLine($"{trigger.RdbTriggerSource}");
            }
        }
    }
}
