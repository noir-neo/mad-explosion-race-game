using System;
using System.Linq;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace GameManagers
{
    class MatchingManager : MonoBehaviour, IMainGameStarter
    {
        [SerializeField] private int _inputCount;
        [SerializeField] private Players.PlayerFactory _playerFactory;

        private Subject<Unit> _startMainGame = new Subject<Unit>();
        public IObservable<Unit> StartMainGameAsObservable()
        {
            return _startMainGame;
        }

        void Start()
        {
            var inputEventProviders = AddInputEventProviders();

            inputEventProviders
                .Select(pair => {
                    return pair.Item2
                        .GetAccelAsObservable()
                        .Where(x => x)
                        .Select(_ => pair.Item1);
                })
                .Merge()
                .Scan(new List<int>(), (history, id) => {
                    return history.Append(id).Distinct().ToList();
                })
                .Skip(1)
                .Take(1)
                .Subscribe(history => {
                    foreach (var id in history) {
                        this.AssignInputToPlayer(id);
                    }
                    _startMainGame.OnNext(Unit.Default);
                });
        }

        void AssignInputToPlayer(int inputId)
        {
            _playerFactory.CreateHuman(inputId);
        }

        List<Tuple<int, Players.InputImpls.HumanInputEventProvider>> AddInputEventProviders()
        {
            return Enumerable.Range(1, _inputCount)
                .Select(id => {
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
    }
}