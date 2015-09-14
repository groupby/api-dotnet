namespace GroupByInc.Api.Injector
{
    public class StaticInjectorFactory<T>
    {
        public StaticInjector<T> Create()
        {
            return new GlobalInjector<T>();
        }
    }
}