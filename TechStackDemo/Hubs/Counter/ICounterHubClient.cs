using TechStackDemo.Repository.Counter;

namespace TechStackDemo.Hubs.Counter
{
    public interface ICounterHubClient
    {
        void AllCountersReset();
        void NewCounterValues(CounterItem[] values);
        
    }
}