using System;
using UniRx;

namespace GameManagers
{
    public interface IMainGameStarter {
        IObservable<Unit> StartMainGameAsObservable();
    }
}