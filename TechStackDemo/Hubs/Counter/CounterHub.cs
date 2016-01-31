using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Serilog;
using TechStackDemo.Config;
using TechStackDemo.Repository;
using TechStackDemo.Repository.Counter;

namespace TechStackDemo.Hubs.Counter
{
    [HubName("counterHub")]
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
            var all = await counterRepository.GetAll();
            var result = all.Where(c => c.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            return result != null ? new CounterItemDto(result) : new CounterItemDto(id);
        }

        public async Task<CounterItemDto> Increment(string id, int qty)
        {
            var result = await counterRepository.Increment(id, qty);

            await Clients.All.NewCounterValue(result.ToDto());

            return new CounterItemDto(result);
        }

        public async Task<CounterItemDto> Decrement(string id, int qty)
        {
            var result = await counterRepository.Increment(id, qty*-1);


            await Clients.All.NewCounterValue(result.ToDto());

            return new CounterItemDto(result);
        }

        public async Task Reset()
        {
            await counterRepository.Reset();

            await Clients.All.AllCountersReset();
        }
    }
}