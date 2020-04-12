using System.IO;
using System.Linq;
using SchemaExtractor.ReferenceTypes;
using SchemaExtractor.Entities;

namespace SchemaExtractor.Writers
{
    public class DependencyWriter
    {
        private readonly TextWriter _writer;

        public DependencyWriter(TextWriter writer)
        {
            _writer = writer;
        }

        public void WriteSection(Dependency[] dependencies, string parentRelationName, string parentType)
        {
            var usesGroup = dependencies
                .Where(a => a.FormatDependentName() == parentRelationName)
                .OrderBy(d => d.FormatDependedOnName())
                .GroupBy(a => a.FormatDependedOnName())
                .ToArray();

            var usedByGroup = dependencies
                .Where(a => a.FormatDependedOnName() == parentRelationName)
                .OrderBy(d => d.FormatDependentName())
                .GroupBy(a => a.FormatDependentName())
                .ToArray();

            _writer.WriteLine("/*");
            _writer.WriteLine($"    {parentType}: {parentRelationName}");
            _writer.WriteLine("    Uses:");

            if (usesGroup.Any())
            {
                foreach (var group in usesGroup)
                {
                    var dependency = group.First();

                    _writer.WriteLine($"        {dependency.FormatDependedOnName()} ({dependency.RdbDependedOnType.FormatName()})");
                }
            }
            else
            {
                _writer.WriteLine($"        <None>");
            }

            _writer.WriteLine("    Used By:");

            if (usedByGroup.Any())
            {
                foreach (var group in usedByGroup)
                {
                    var dependency = group.First();

                    _writer.WriteLine($"        {dependency.FormatDependentName()} ({dependency.RdbDependentType.FormatName()})");
                }
            }
            else
            {
                _writer.WriteLine($"        <None>");
            }

            _writer.WriteLine("*/");
        }
    }
}
