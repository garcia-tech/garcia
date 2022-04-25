namespace Garcia.Application.Contracts.Infrastructure
{
    public interface IGarciaCache
    {
        TItem Set<TItem>(object key, TItem item);
        void Remove(string name);
        TItem Get<TItem>(object key);
    }
}