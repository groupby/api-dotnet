namespace GroupByInc.Api.Requests
{
    public class Request : AbstractRequest<Request>
    {
        private bool? _wildcardSearchEnabled = false;

        private MatchStrategy _matchStrategy;

        public bool? IsWildcardSearchEnabled()
        {
            return _wildcardSearchEnabled;
        }

        public Request SetWildcardSearchEnabled(bool? wildcardSearchEnabled)
        {
            _wildcardSearchEnabled = wildcardSearchEnabled;
            return this;
        }

        public MatchStrategy GetMatchStrategy()
        {
            return _matchStrategy;
        }

        public Request SetMatchStrategy(MatchStrategy matchStrategy)
        {
            _matchStrategy = matchStrategy;
            return this;
        }
    }
}
