using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Nito.AsyncEx;
using Serilog;
using TechStackDemo.Utils;

namespace TechStackDemo.Repository.Counter
{
    public class CounterRepository:ICounterRepository, IChangeSubscriber<CounterItem>
    {
        private readonly ILogger logger;
        private readonly AsyncReaderWriterLock rwLock = new AsyncReaderWriterLock();
        private ImmutableDictionary<string, CounterItem> data = ImmutableDictionary.Create<string, CounterItem>(StringComparer.InvariantCultureIgnoreCase);
        private readonly Subject<CounterItem> changes = new Subject<CounterItem>();
        private readonly Subject<Unit> reset = new Subject<Unit>();

        public CounterRepository(ILogger logger)
        {
            this.logger = logger.ForContext("Repo", "Counter");
        }

        public async Task<IEnumerable<CounterItem>> GetAll()
        {
            logger.Information("Counter GetAll invoked");
            using (await rwLock.ReaderLockAsync().ConfigureAwait(false))
            {
                logger.Information("Counter GetAll in lock");
                return data.Values;
            }
        }

        public async Task<CounterItem> Increment(string counter, int qty)
        {
            logger.Information("Counter Increment invoked");
            using (await rwLock.WriterLockAsync().ConfigureAwait(false))
            {
                logger.Information("Counter Increment in lock");

                //busy work!
                //await Task.Delay(TimeSpan.FromMilliseconds(SafeRandom.Next(5000))).ConfigureAwait(false);

                logger.Information("Counter increment - work done");

                var prev = data.GetValueOrDefault(counter) ?? new CounterItem(counter, 0);
                var next = prev.WithValue(prev.Value + qty);
                data = data.SetItem(counter, next);

                NotifyChange(next);

                logger.Information("Counter updated from {@old} to {@new}", prev, next);

                return next;
            }
        }

        public async Task Reset()
        {
            logger.Error("Performing  a reset!");


            using (await rwLock.WriterLockAsync().ConfigureAwait(false))
            {
                //await Task.Delay(TimeSpan.FromSeconds(8)).ConfigureAwait(false);
                var old = data;
                data = data.Clear();

                foreach (var item in old.Values)
                {
                    NotifyChange(item.WithValue(0));
                }
            }

            reset.OnNext(Unit.Default);
        }

        public async Task Remove(string counter)
        {
            using (await rwLock.WriterLockAsync().ConfigureAwait(false))
            {
                //await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);

                if (data.ContainsKey(counter))
                {
                    var item = data[counter];
                    data = data.Remove(counter);
                    NotifyChange(item);
                }
            }
        }

        public IObservable<CounterItem> GetChangesObservable()
        {
            return changes.AsObservable().ObserveOn(TaskPoolScheduler.Default);
        }

        public IObservable<Unit> GetResetObservable()
        {
            return reset.AsObservable().ObserveOn(TaskPoolScheduler.Default);
        } 

        private void NotifyChange(CounterItem item)
        {
            changes.OnNext(item);
        }
    }

    
}