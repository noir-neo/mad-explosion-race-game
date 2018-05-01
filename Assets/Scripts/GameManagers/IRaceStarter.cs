using System;
using UniRx;

namespace GameManagers
{
    public interface IRaceStarter {
        IObservable<Unit> StartRaceAsObservable();
    }
}