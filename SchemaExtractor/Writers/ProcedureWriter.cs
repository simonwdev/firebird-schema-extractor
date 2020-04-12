using System.IO;
using System.Linq;
using SchemaExtractor.Entities;

namespace SchemaExtractor.Writers
{
    public class ProcedureWriter
    {
        private readonly TextWriter _writer;
        private readonly CharacterSet[] _characterSets;

        public ProcedureWriter(TextWriter writer, CharacterSet[] characterSets)
        {
            _characterSets = characterSets;
            _writer = writer;
        }

        public void Write(Procedure procedure)
        {
            var characterSetDictionary = _characterSets.ToDictionary(a => a.RdbCharacterSetId ?? 0);
            var procedureName = procedure.FormatName();
            
            _writer.Write($"CREATE OR ALTER PROCEDURE {procedureName}");
            
            var inputParameters = procedure.RdbProcedureParameters
                .Where(a => a.RdbParameterType == ParameterType.Input)
                .OrderBy(a => a.RdbParameterNumber)
                .ToArray();

            var outputParameters = procedure.RdbProcedureParameters
                .Where(a => a.RdbParameterType == ParameterType.Output)
                .OrderBy(a => a.RdbParameterNumber)
                .ToArray();

            if (inputParameters.Any())
            {
                _writer.Write(" (");
                _writer.WriteLine();

                foreach (var parameter in inputParameters)
                {
                    var parameterName = parameter.FormatName();
                    var fieldName = parameter.RdbField.FormatName();

                    _writer.Write($"    {parameterName}");

                    if (parameter.IsExplicitDomain())
                    {
                        _writer.Write($" {fieldName}");
                    }
                    else
                    {
                        _writer.Write(" ");

                        new FieldWriter(_writer, characterSetDictionary).WriteType(field: parameter.RdbField,
                            parentDefaultSource: parameter.RdbDefaultSource,
                            parentIsNullable: parameter.RdbNullFlag ?? false);
                    }

                    if (parameter != inputParameters.Last())
                    {
                        _writer.Write(", ");
                        _writer.WriteLine();
                    }
                }

                _writer.WriteLine();
                _writer.Write(")");
            }

            if (outputParameters.Any())
            {
                _writer.WriteLine();
                _writer.Write("RETURNS (");
                _writer.WriteLine();

                foreach (var parameter in outputParameters)
                {
                    var parameterName = parameter.FormatName();
                    var fieldName = parameter.RdbField.FormatName();

                    _writer.Write($"    {parameterName}");

                    if (parameter.IsExplicitDomain())
                    {
                        _writer.Write($" {fieldName}");
                    }
                    else
                    {
                        _writer.Write(" ");

                        new FieldWriter(_writer, characterSetDictionary).WriteType(field: parameter.RdbField,
                            parentDefaultSource: parameter.RdbDefaultSource,
                            parentIsNullable: parameter.RdbNullFlag ?? false);
                    }

                    if (parameter.RdbDefaultSource != null)
                    {
                        _writer.Write(" ");
                        _writer.Write(parameter.RdbDefaultSource);
                    }
                    else if (parameter.RdbDefaultSource != null)
                    {
                        _writer.Write(" ");
                        _writer.Write(parameter.RdbDefaultSource);
                    }

                    if (parameter.RdbNullFlag ?? false)
                    {
                        // NOTE: ISQL has code here to deal with CONSTRAINTS on a field level
                        // which don't seem to be used in Communicare.
                        _writer.Write(" NOT NULL");
                    }

                    var collationId = parameter.RdbCollationId ?? parameter.RdbCollationId ?? 0;
                    if (collationId > 0)
                    {
                        // Not currently used in Communicare.
                        _writer.Write(" #MISSING#(Collation)");
                    }

                    if (parameter != outputParameters.Last())
                    {
                        _writer.Write(", ");
                        _writer.WriteLine();
                    }
                }

                _writer.Write(")");
            }

            _writer.WriteLine();
            _writer.Write("AS");
            _writer.WriteLine();

            if (procedure.RdbProcedureSource != null)
            {
                _writer.Write(procedure.RdbProcedureSource);
            }
        }
    }
}
