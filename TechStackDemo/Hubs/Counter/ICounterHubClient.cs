using System.Threading.Tasks;
using TechStackDemo.Repository.Counter;

namespace TechStackDemo.Hubs.Counter
{
    public interface ICounterHubClient
    {
        Task AllCountersReset();
        Task NewCounterValue(CounterItemDto values);
        
    }
}