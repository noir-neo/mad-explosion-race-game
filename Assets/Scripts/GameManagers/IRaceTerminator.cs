using System;
using UniRx;

namespace GameManagers
{
    interface IRaceTerminator
    {
        IObservable<Unit> TerminateRaceAsObservable();
    }
}