using System.Collections.Generic;
using SchemaExtractor.Extensions;
using System.IO;
using SchemaExtractor.Entities;
using SchemaExtractor.ReferenceTypes;

namespace SchemaExtractor.Writers
{
    public class FieldWriter
    {
        private readonly TextWriter _writer;
        private readonly Dictionary<int, CharacterSet> _characterSetDictionary;

        public FieldWriter(TextWriter writer, Dictionary<int, CharacterSet> characterSetDictionary)
        {
            _writer = writer;
            _characterSetDictionary = characterSetDictionary;
        }

        public void WriteType(Field field, string parentDefaultSource, bool parentIsNullable)
        {
            var precisionKnown = false;

            var fieldType = field.RdbFieldType ?? FieldType.Unknown;
            var fieldTypeName = fieldType.FormatName();
            var sqlSubType = (NumericFieldSubType)(field.RdbFieldSubType ?? 0);

            if (fieldType == FieldType.SmallInt || fieldType == FieldType.Integer || fieldType == FieldType.BigInt)
            {
                if (field.RdbFieldPrecision != null)
                {
                    var numericSubType = field.SubTypeAsNumeric();

                    if (numericSubType != NumericFieldSubType.Unknown)
                    {
                        precisionKnown = true;

                        _writer.Write($"{numericSubType.FormatName()}({field.RdbFieldPrecision}, {-field.RdbFieldScale})");
                    }
                }
            }

            if (!precisionKnown)
            {
                // Take a stab at numerics and decimals
                if (field.RdbFieldType == FieldType.SmallInt && field.RdbFieldScale < 0)
                {
                    _writer.Write($"NUMERIC(4, {-field.RdbFieldScale})");
                }
                else if (field.RdbFieldType == FieldType.Integer && field.RdbFieldScale < 0)
                {
                    _writer.Write($"NUMERIC(9, {-field.RdbFieldScale})");
                }
                else if (field.RdbFieldType == FieldType.DoublePrecision && field.RdbFieldScale < 0)
                {
                    _writer.Write($"NUMERIC(15, {-field.RdbFieldScale})");
                }
                else
                {
                    _writer.Write(fieldTypeName);
                }
            }

            if (field.RdbFieldType == FieldType.Char || field.RdbFieldType == FieldType.Varchar)
            {
                if (field.RdbCharacterLength == null)
                {
                    _writer.Write($"({field.RdbFieldLength})");
                }
                else
                {
                    _writer.Write($"({field.RdbCharacterLength})");
                }
            }

            if (field.RdbDimensions != null)
            {
                // NOT IMPLEMENTED
                _writer.Write(" #MISSING#");
            }

            if (field.RdbFieldType == FieldType.Blob)
            {
                var blobSubType = field.SubTypeAsBlob();

                _writer.Write($" SUB_TYPE {blobSubType.FormatName()}");
                _writer.Write($" SEGMENT SIZE {field.RdbSegmentLength}");
            }
            
            if ((fieldType == FieldType.Char || fieldType == FieldType.Varchar || fieldType == FieldType.Blob) && (field.RdbCharacterSetId ?? 0) > 0)
            {
                var characterSet = _characterSetDictionary.GetValueOrDefault(field.RdbCharacterSetId ?? 0);
                if (characterSet != null)
                {
                    var characterSetName = characterSet.FormatName();
                    if (characterSetName != "NONE" && characterSetName != "ASCII")
                        _writer.Write($" CHARACTER SET {characterSetName}");
                }
                else
                {
                    _writer.Write(" CHARACTER SET #MISSING#");
                }
            }
        }
    }
}
