using System.Collections.Generic;
using System.Threading.Tasks;

namespace TechStackDemo.Repository.Counter
{
    public interface ICounterRepository
    {
        Task<IEnumerable<CounterItem>> GetAll();
        Task<CounterItem> Increment(string counter, int qty);
        Task Reset();
        Task Reset(string counter);
    }
}