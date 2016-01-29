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
    public class CounterHubManager: IStartable, IDisposable
    {
        private readonly ILogger logger;
        private readonly IChangeSubscriber<CounterItem> counterItemChangesSubscriber;
        private IDisposable toDispose = new CompositeDisposable();
        private bool disposed = false;

        private readonly Lazy<IHubContext<ICounterHubClient>> counterHubContext =
            new Lazy<IHubContext<ICounterHubClient>>(
                () => Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<ICounterHubClient>("CounterHub")
                );

        public CounterHubManager(ILogger logger, IChangeSubscriber<CounterItem> counterItemChangesSubscriber )
        {
            this.logger = logger;
            this.counterItemChangesSubscriber = counterItemChangesSubscriber;
        }

        public void Start()
        {
            var subscription = counterItemChangesSubscriber.GetChangesObservable()
                .Do(change=>logger.Warning("Change noticed for {@counter}", change))
                .Where(c => !c.Id.EndsWith("BrokenNotify", StringComparison.InvariantCultureIgnoreCase))
                .Buffer(TimeSpan.FromSeconds(2), 5)
                .Where(bufferedChanges => bufferedChanges.Any())
                .Subscribe(bufferedChanges =>
                {
                    counterHubContext.Value.Clients.All.NewCounterValues(bufferedChanges.ToArray());
                });
            toDispose = new CompositeDisposable(toDispose, subscription);
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