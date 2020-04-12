using System.IO;
using System.Linq;
using SchemaExtractor.Entities;

namespace SchemaExtractor.Writers
{
    public class ViewWriter
    {
        private readonly TextWriter _writer;

        public ViewWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public void Write(Relation relation)
        {
            var viewName = relation.FormatName();

            _writer.Write($"CREATE OR ALTER VIEW {viewName} (");
            _writer.WriteLine();

            var fields = relation.RdbRelationFields
                .OrderBy(a => a.RdbFieldPosition)
                .ToArray();

            foreach (var field in fields)
            {
                var fieldName = field.FormatName();

                _writer.Write($"    {fieldName}");

                if (field != fields.Last())
                {
                    _writer.Write(", ");
                }

                _writer.WriteLine();
            }

            _writer.WriteLine(")");
            _writer.Write("AS");
            _writer.Write(relation.RdbViewSource);
            _writer.WriteLine(";");
        }
    }
}
