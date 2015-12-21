namespace GroupByInc.Api.Injector
{
    public class GlobalInjector<T> : StaticInjector<T>
    {
        private T _injectedObject;

        public T Get()
        {
            return _injectedObject;
        }

        public void Set(T injectedObject)
        {
            _injectedObject = injectedObject;
        }
    }
}