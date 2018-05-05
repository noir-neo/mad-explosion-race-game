using System.Collections.Generic;
using System;

namespace GameManagers
{
    public interface ICheckPointPassengersProvider
    {
        IObservable<List<ICheckPointPassenger>> CheckPointPassengersAsObservable();
    }
}