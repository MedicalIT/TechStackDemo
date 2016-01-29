using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Serilog;
using TechStackDemo.Repository;
using TechStackDemo.Repository.Counter;

namespace TechStackDemo.Hubs.Counter
{
    public class CounterHub: Hub<ICounterHubClient>
    {
        private readonly ILogger logger;
        private readonly ICounterRepository counterRepository;

        public CounterHub(ILogger logger, ICounterRepository counterRepository)
        {
            this.logger = logger;
            this.counterRepository = counterRepository;
        }

        public async Task<CounterItemDto> GetCounterItem(string id)
        {
            //subscribe
            await Groups.Add(Context.ConnectionId, id.ToLowerInvariant());

            var all = await counterRepository.GetAll();
            var result = all.Where(c => c.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            return result != null ? new CounterItemDto(result) : new CounterItemDto(id);
        }

        public async Task<CounterItemDto> Increment(string id, int qty)
        {
            //subscribe
            await Groups.Add(Context.ConnectionId, id.ToLowerInvariant());

            return new CounterItemDto(await counterRepository.Increment(id, qty));
        }

        public async Task<CounterItemDto> Decrement(string id, int qty)
        {
            //subscribe
            await Groups.Add(Context.ConnectionId, id.ToLowerInvariant());
            return new CounterItemDto(await counterRepository.Increment(id, qty*-1));
        }
    }
}