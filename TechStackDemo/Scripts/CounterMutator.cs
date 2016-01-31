using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Serilog;
using TechStackDemo.Repository.Counter;
using TechStackDemo.Utils;

namespace TechStackDemo.Scripts
{
    public class CounterMutator: IStartable, IDisposable
    {
        private readonly ICounterRepository counterRepo;
        private readonly ILogger logger;
        private bool disposed = false;
        private IDisposable tickerDisposable = null;
        
        public CounterMutator(ICounterRepository counterRepo, ILogger logger)
        {
            this.counterRepo = counterRepo;
            this.logger = logger;
        }

        private async Task OnTick()
        {
            var counters = await counterRepo.GetAll();

            //var countersToIncrement = counters.Where(c => c.Value < 20 || SafeRandom.Next(10) > 2)
            //    .Select(c=>new {Counter = c, AdjustmentQty = SafeRandom.Next(3) + 1})
            //    .ToList();
            //logger.Warning("Mutator affecting counters {@counters}", countersToIncrement);
            //var incrementTasks = countersToIncrement.Select(c => counterRepo.Increment(c.Counter.Id, c.AdjustmentQty));
            //await Task.WhenAll(incrementTasks);

            //OR

            foreach (var counter in counters)
            {
                if (counter.Value < 3 || SafeRandom.Next(10) >= 5)
                {
                    await counterRepo.Increment(counter.Id, SafeRandom.Next(3));
                }
            }
        }

        public void Start()
        {
            tickerDisposable = Observable.Interval(TimeSpan.FromSeconds(5))
                .SubscribeAsync(_ => OnTick());
        }

        public void Stop()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                tickerDisposable?.Dispose();
            }
        }
    }

    public class CounterMutatorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<CounterMutator>().LifestyleSingleton());
        }
    }
}