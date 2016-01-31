using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TechStackDemo.Utils
{
    public static class RxExtensions
    {

        public static IObservable<OUT> SelectAsync<IN, OUT>(this IObservable<IN> ob, Func<IN, Task<OUT>> asyncFunc)
        {
            return ob.SelectMany(element => Observable.FromAsync<OUT>(() => asyncFunc(element)));
        }

        public static IObservable<Unit> SelectAsync<IN>(this IObservable<IN> ob, Func<IN, Task> asyncFunc)
        {
            return
                ob.SelectMany(
                    element => Observable.FromAsync<Unit>(() => asyncFunc(element).ContinueWith(t => Unit.Default, TaskContinuationOptions.OnlyOnRanToCompletion)));
        }

        public static IDisposable SubscribeAsync<IN>(this IObservable<IN> ob, Func<IN, Task> asyncFunc)
        {
            return ob.SelectAsync(asyncFunc).Subscribe(unit => { });
        }

    }
}