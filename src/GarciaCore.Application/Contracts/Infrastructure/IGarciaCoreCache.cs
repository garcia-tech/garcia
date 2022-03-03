namespace GarciaCore.Application.Contracts.Infrastructure
{
    public interface IGarciaCoreCache
    {
        TItem Set<TItem>(object key, TItem item);
        void Remove(string name);
        TItem Get<TItem>(object key);
    }
}