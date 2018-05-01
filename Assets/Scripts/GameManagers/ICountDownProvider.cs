using System;
using UniRx;

namespace GameManagers
{
    public interface ICountDownProvider {
        IObservable<int> CountDownAsObservable();
    }
}