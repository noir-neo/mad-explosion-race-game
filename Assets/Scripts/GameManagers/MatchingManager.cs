using System;
using System.Linq;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace GameManagers
{
    class MatchingManager : MonoBehaviour, IMainGameStarter, IHumanInputProvider
    {
        [SerializeField] private int _inputCount;

        private ISubject<Unit> _startMainGame = new AsyncSubject<Unit>();

        private ISubject<List<int>> _activeHumanInputIds = new AsyncSubject<List<int>>();

        void Start()
        {
            var inputEventProviders = AddInputEventProviders();

            inputEventProviders
                .Select(pair =>
                {
                    return pair.Item2
                        .GetAccelAsObservable()
                        .Where(x => x)
                        .Select(_ => pair.Item1);
                })
                .Merge()
                .Scan(new List<int>(), (history, id) =>
                {
                    return history.Append(id).Distinct().ToList();
                })
                .Skip(1)
                .Take(1)
                .Subscribe(inputIds =>
                {
                    _startMainGame.OnNext(Unit.Default);
                    _startMainGame.OnCompleted();
                    _activeHumanInputIds.OnNext(inputIds);
                    _activeHumanInputIds.OnCompleted();
                });
        }

        List<Tuple<int, Players.InputImpls.HumanInputEventProvider>> AddInputEventProviders()
        {
            return Enumerable.Range(1, _inputCount)
                .Select(id =>
                {
                    return Tuple.Create(id, AddInputEventProvider(id));
                })
                .ToList();
        }

        Players.InputImpls.HumanInputEventProvider AddInputEventProvider(int inputId)
        {
            var inputEventProvider = this.gameObject.AddComponent<Players.InputImpls.HumanInputEventProvider>();
            inputEventProvider.Inject(inputId);
            return inputEventProvider;
        }

        public IObservable<Unit> StartMainGameAsObservable()
        {
            return _startMainGame;
        }

        public IObservable<List<int>> HumanInputIdsAsObservable()
        {
            return _activeHumanInputIds;
        }
    }
}