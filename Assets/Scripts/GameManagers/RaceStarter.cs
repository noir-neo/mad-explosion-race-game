using System;
using UnityEngine;
using UniRx;
using Zenject;

namespace GameManagers
{
    class RaceStarter : MonoBehaviour, IRaceStarter, ICountDownProvider
    {
        [SerializeField] private int countDownStartsFrom;

        [Inject] private IMainGameStarter mainGameStarter;

        public IObservable<int> CountDownAsObservable()
        {
            return mainGameStarter
                .StartMainGameAsObservable()
                .SelectMany(_ => Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1)))
                .Select(count => countDownStartsFrom - (int)count)
                .Take(countDownStartsFrom + 1);
        }

        public IObservable<Unit> StartRaceAsObservable()
        {
            return CountDownAsObservable()
                .Skip(countDownStartsFrom)
                .First()
                .Select(_ => Unit.Default);
        }
    }
}