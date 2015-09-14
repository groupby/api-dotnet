namespace GroupByInc.Api.Injector
{
    public interface StaticInjector<T>
    {
        T Get();
        void Set(T injectedObject);
    }
}