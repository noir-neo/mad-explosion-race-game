using System;
using UniRx;

namespace GameManagers
{
    public interface ICheckPointPassenger
    {
        IObservable<CheckPoint> PassedCheckPointAsObservable();
    }
}