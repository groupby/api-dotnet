namespace GroupByInc.Api.Requests
{
    /// <summary>
    /// </summary>
    public class RestrictNavigation
    {
        private string _name;
        private int _count;

        public RestrictNavigation SetName(string value)
        {
            _name = value;
            return this;
        }

        public string GetName()
        {
            return _name;
        }

        public RestrictNavigation SetCount(int value)
        {
            _count = value;
            return this;
        }

        public int GetCount()
        {
            return _count;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "[" + _name + "," + _count + "]";
        }
    }
}