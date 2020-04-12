using System.IO;
using System.Linq;
using SchemaExtractor.Entities;

namespace SchemaExtractor.Writers
{
    public class TableWriter
    {
        private readonly TextWriter _writer;
        private readonly CharacterSet[] _characterSets;
        private readonly Collation[] _collations;

        public TableWriter(TextWriter writer, CharacterSet[] characterSets, Collation[] collations)
        {
            _characterSets = characterSets;
            _collations = collations;
            _writer = writer;
        }

        public void Write(Relation relation)
        {
            var characterSetDictionary = _characterSets.ToDictionary(a => a.RdbCharacterSetId ?? 0);
            var tableName = relation.FormatName();

            string tableTypeName;
            switch (relation.RdbRelationType)
            {
                case RelationType.GlobalTempPreserve:
                case RelationType.GlobalTempDelete:
                    tableTypeName = "GLOBAL TEMPORARY TABLE";
                    break;
                default:
                    tableTypeName = "TABLE";
                    break;
            }

            _writer.Write($"CREATE {tableTypeName} {tableName}");

            if (relation.RdbExternalFile != null)
                _writer.Write($" EXTERNAL FILE {relation.RdbExternalFile}");

            _writer.Write(" (");

            _writer.WriteLine();

            var relationFields = relation.RdbRelationFields
                .OrderBy(a => a.RdbFieldPosition)
                .ToArray();

            foreach (var relationField in relationFields)
            {
                var relationFieldName = relationField.FormatName();
                var field = relationField.RdbField;
                var fieldName = field.FormatName();
                
                _writer.Write($"    {relationFieldName}");

                if (field.IsComputed())
                {
                    _writer.Write(" COMPUTED BY");
                    _writer.Write(field.RdbComputedSource != null ? $" {field.RdbComputedSource}" : " #MISSING#");
                }
                else if (field.IsExplicitDomain())
                {
                    _writer.Write($" {fieldName}");
                }
                else
                {
                    _writer.Write(" ");

                    new FieldWriter(_writer, characterSetDictionary).WriteType(field,
                        parentDefaultSource: relationField.RdbDefaultSource,
                        parentIsNullable: relationField.RdbNullFlag ?? false);

                    if (relationField.RdbDefaultSource != null)
                    {
                        _writer.Write(" ");
                        _writer.Write(relationField.RdbDefaultSource);
                    }
                    else if (field.RdbDefaultSource != null)
                    {
                        _writer.Write(" ");
                        _writer.Write(field.RdbDefaultSource);
                    }
                }
                
                if (relationField.RdbNullFlag ?? false)
                {
                    _writer.Write(" NOT NULL");
                }

                var collationId = relationField.RdbCollationId ?? field.RdbCollationId ?? 0;
                if (collationId > 0)
                {
                    // Not currently used in Communicare.
                    _writer.Write(" #MISSING#(Collation)");
                }

                if (relationField != relationFields.Last())
                {
                    _writer.Write(", ");
                }

                _writer.WriteLine();
            }

            if (relation.RdbRelationType == RelationType.GlobalTempPreserve)
            {
                _writer.WriteLine(")");
                _writer.WriteLine(" ON COMMIT PRESERVE ROWS");
            }
            else if (relation.RdbRelationType == RelationType.GlobalTempDelete)
            {
                _writer.WriteLine(")");
                _writer.WriteLine(" ON COMMIT DELETE ROWS");
            }
            else
            {
                _writer.WriteLine(")");
            }

            _writer.WriteLine();
        }
    }
}
