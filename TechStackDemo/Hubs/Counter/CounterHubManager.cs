using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Web;
using Castle.Core;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Serilog;
using TechStackDemo.Repository.Counter;
using TechStackDemo.Utils;

namespace TechStackDemo.Hubs.Counter
{
    public class CounterHubManager: HubManagerBase, IStartable, IDisposable
    {
        private readonly ILogger logger;
        private readonly IChangeSubscriber<CounterItem> counterItemChangesSubscriber;
        private IDisposable toDispose = new CompositeDisposable();
        private bool disposed = false;

        private IHubContext<ICounterHubClient> GetCounterHubContext()
        {
            var counterHubContext = GlobalHost.ConnectionManager.GetHubContext<ICounterHubClient>("CounterHub");
            return counterHubContext;
        }

        public CounterHubManager(ILogger logger, IChangeSubscriber<CounterItem> counterItemChangesSubscriber )
        {
            this.logger = logger;
            this.counterItemChangesSubscriber = counterItemChangesSubscriber;
        }

        public void Start()
        {
            var counterSubscription = counterItemChangesSubscriber.GetChangesObservable()
                .Do(change=>logger.Warning("Change noticed for {@counter}", change))
                .Where(c => !c.Id.EndsWith("BrokenNotify", StringComparison.InvariantCultureIgnoreCase))
                .Buffer(TimeSpan.FromSeconds(2), 5)
                .Where(bufferedChanges => bufferedChanges.Any())
                .SubscribeAsync(async bufferedChanges =>
                {
                    var counterHubContext = this.GetCounterHubContext();

                    foreach (var bufferedChange in bufferedChanges)
                    {
                        //await counterHubContext.Clients.Group("X").NewCounterValue(bufferedChange.ToDto());
                        await counterHubContext.Clients.All.NewCounterValue(bufferedChange.ToDto());
                    }
                });

            var resetSubscription = counterItemChangesSubscriber.GetResetObservable()
                .SubscribeAsync(_ =>
                {
                    var counterHubContext = this.GetCounterHubContext();
                    return counterHubContext.Clients.All.AllCountersReset();
                });

            toDispose = new CompositeDisposable(toDispose, counterSubscription, resetSubscription);
        }

        public void Stop()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;

                toDispose.Dispose();
            }
        }
    }
}