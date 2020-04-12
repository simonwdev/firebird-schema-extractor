using JetBrains.Annotations;

namespace SchemaExtractor.Entities
{
    [UsedImplicitly]
    public class IndexSegment
    {
        /// <Summary>
        /// The name of the index this segment is related to. The master record is RDBINDICES.RDBINDEXNAME.
        /// </Summary>
        public string RdbIndexName { get; set; }
        /// <Summary>
        /// The name of a column belonging to the index, corresponding to an identifier for the table and that column in RDBRELATIONFIELDS.RDBFIELDNAME
        /// </Summary>
        public string RdbFieldName { get; set; }
        /// <Summary>
        /// The column position in the index. Positions are numbered left-to-right, starting at zero
        /// </Summary>
        public int? RdbFieldPosition { get; set; }
        /// <Summary>
        /// The last known (calculated) selectivity of this column in the index. The higher the number, the lower the selectivity. 
        /// </Summary>
        public string RdbStatistics { get; set; }

        public virtual Index RdbIndex { get; set; }

        public string FormatFieldName()
        {
            return (RdbFieldName ?? string.Empty).Trim();
        }
    }
}
