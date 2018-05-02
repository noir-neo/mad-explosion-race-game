using System;
using System.Collections.Generic;
using System.Linq;
using AIs;
using Players.InputImpls;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
using GameManagers;
using UniRx;

namespace Players
{
    class PlayerFactory : MonoBehaviour
    {
        [SerializeField] private List<GameObject> machines;
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private Vector3 _firstGridPosition;
        [SerializeField] private Vector2 _gridInterval;

        [Inject] private DiContainer diContainer;

        [Inject] private CoursePath coursePath;

        [Inject] private IHumanInputProvider humanInputProvider;

        private List<PlayerCore> players;

        void Start()
        {
            players = CreatePlayers();
            humanInputProvider
                .HumanInputIdsAsObservable()
                .Subscribe(AssignInputToPlayers)
                .AddTo(this);
        }

        private PlayerCore CreatePlayer(Transform transform)
        {
            int i = Random.Range(0, machines.Count);
            var player = diContainer.InstantiatePrefab(machines[i], transform);

            return player.GetComponent<PlayerCore>();
        }

        private List<PlayerCore> CreatePlayers()
        {
            return Enumerable.Range(0, _gridSize.y)
                .SelectMany(y =>
                {
                    return Enumerable.Range(0, _gridSize.x)
                        .Select(x => new Vector2(x, y));
                })
                .Select(grid =>
                {
                    var transform = (new GameObject()).transform;
                    transform.position = _firstGridPosition;

                    var offset = _gridInterval.X0Y();
                    offset.Scale(grid.X0Y());

                    transform.position += offset;

                    return CreatePlayer(transform);
                })
                .ToList();
        }

        private void AssignInputToPlayers(List<int> humanInputIds)
        {
            var aiPlayers = players.Take(players.Count - humanInputIds.Count);
            foreach (var player in aiPlayers)
            {
                var inputEventProvider = player.gameObject.AddComponent<AiInputEventProvider>();
                inputEventProvider.Inject(coursePath);
                player.Configure(inputEventProvider);
            }

            var humanPlayers = players.TakeLast(humanInputIds.Count).Zip(humanInputIds, Tuple.Create);
            foreach (var t in humanPlayers.Select((x, i) => Tuple.Create(x.Item1, x.Item2, i)))
            {
                var inputEventProvider = t.Item1.gameObject.AddComponent<HumanInputEventProvider>();
                inputEventProvider.Inject(t.Item2);
                t.Item1.Configure(inputEventProvider, t.Item3 + 1);
            }
        }
    }
}