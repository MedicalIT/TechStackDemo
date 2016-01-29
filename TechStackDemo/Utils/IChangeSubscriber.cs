using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace TechStackDemo.Utils
{
    public interface IChangeSubscriber<T>
    {
        IObservable<T> GetChangesObservable();
    }
}