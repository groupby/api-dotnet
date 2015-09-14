namespace GroupByInc.Api.Models
{
    /// <summary>
    ///     The <see cref="Cluster" /> record is a simpler record type than a main
    ///     record.
    /// </summary>
    public class ClusterRecord
    {
        /// <summary>
        ///     The title of this record.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     The Unique identifier of this record.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     The matching set of terms for this record.
        /// </summary>
        public string Snippet { get; set; }
    }
}