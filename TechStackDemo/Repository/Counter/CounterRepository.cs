using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
                await Task.Delay(TimeSpan.FromMilliseconds(SafeRandom.Next(5000))).ConfigureAwait(false);

                logger.Information("Counter increment - work done");

                var prev = data.GetValueOrDefault(counter) ?? new CounterItem(counter, 0);
                var next = prev.WithValue(prev.Value + qty);

                logger.Information("Counter updated from {@old} to {@new}", prev, next);

                return next;
            }
        }

        public async Task Reset()
        {
            using (await rwLock.WriterLockAsync().ConfigureAwait(false))
            {
                await Task.Delay(TimeSpan.FromSeconds(8)).ConfigureAwait(false);

                data = data.Clear();
            }
        }

        public async Task Reset(string counter)
        {
            using (await rwLock.WriterLockAsync().ConfigureAwait(false))
            {
                await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);
                data = data.Remove(counter);
            }
        }

        public IObservable<CounterItem> GetChangesObservable()
        {
            return changes.AsObservable();
        }
    }
}