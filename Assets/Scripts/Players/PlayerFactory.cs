using System.Collections.Generic;
using System.Linq;
using System;
using AIs;
using Cinemachine;
using Players.InputImpls;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Players
{
    class PlayerFactory : MonoBehaviour
    {
        [SerializeField] private List<GameObject> machines;
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private Vector3 _firstGridPosition;
        [SerializeField] private Vector2 _gridInterval;

        [Inject] private CoursePath coursePath;

        private List<PlayerCore> players;

        void Start()
        {
            players = CreatePlayers();
        }

        private PlayerCore CreatePlayer()
        {
            return CreatePlayer(Vector3.zero, Quaternion.identity);
        }

        private PlayerCore CreatePlayer(Vector3 position, Quaternion rotation)
        {
            int i = Random.Range(0, machines.Count);
            var player = Instantiate(machines[i], position, rotation);

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
                    var offset = _gridInterval.X0Y();
                    offset.Scale(grid.X0Y());
                    var position = _firstGridPosition + offset;
                    return CreatePlayer(position, Quaternion.identity);
                })
                .ToList();
        }

        public void AssignInputToPlayers(List<int> humanInputIds)
        {
            var aiPlayers = players.Take(players.Count - humanInputIds.Count);
            foreach (var player in aiPlayers)
            {
                var inputEventProvider = player.gameObject.AddComponent<AiInputEventProvider>();
                inputEventProvider.Inject(coursePath);
                player.Configure(inputEventProvider, false);
            }

            var humanPlayers = players.Skip(aiPlayers.Count());
            for (var i = 0; i < humanPlayers.Count(); i++) {
                var player = humanPlayers.ElementAt(i);
                var inputId = humanInputIds[i];

                var inputEventProvider = player.gameObject.AddComponent<HumanInputEventProvider>();
                inputEventProvider.Inject(inputId);
                player.Configure(inputEventProvider, true);
            }
        }
    }
}