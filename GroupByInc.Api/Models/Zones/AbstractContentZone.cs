namespace GroupByInc.Api.Models.Zones
{
    public abstract class AbstractContentZone<T> : Zone
        where T : AbstractContentZone<T>
    {
        private string _content;

        public string GetContent()
        {
            return _content;
        }

        protected virtual T SetContent(string content)
        {
            _content = content;
            return (T) this;
        }
    }
}