using System;
using System.Collections.Generic;
using UniRx;

namespace GameManagers
{
    public interface IHumanInputProvider {
        IObservable<List<int>> HumanInputIdsAsObservable();
    }
}