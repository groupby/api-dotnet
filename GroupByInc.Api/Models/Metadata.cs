namespace GroupByInc.Api.Models
{
    /// <summary>
    ///     Metadata is associated with Navigation objects and Areas and allows the
    ///     merchandiser, from the command center to add additional information
    ///     about a navigation or area. For example there might be a UI hint that
    ///     the price range navigation should be displayed as a slider. Or you might
    ///     set an area metadata to inform the UI of the seasonal color scheme to
    ///     use.
    /// </summary>
    public class Metadata
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}