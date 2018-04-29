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

        [Inject] private GameManagers.IMainGameStarter _mainGameStarter;

        private List<PlayerCore> players;

        void Start()
        {
            players = CreatePlayers();
        }

        public PlayerCore CreateHuman(int inputId)
        {
            var player = CreatePlayer();
            var inputEventProvider = player.gameObject.AddComponent<HumanInputEventProvider>();
            inputEventProvider.Inject(inputId);
            player.Configure(inputEventProvider, true);
            return player;
        }

        private PlayerCore CreateAi()
        {
            var player = CreatePlayer(new Vector3(5, 0, 0), Quaternion.identity);
            var inputEventProvider = player.gameObject.AddComponent<AiInputEventProvider>();
            inputEventProvider.Inject(coursePath);
            player.Configure(inputEventProvider);
            return player;
        }

        private PlayerCore CreatePlayer()
        {
            return CreatePlayer(Vector3.zero, Quaternion.identity);
        }

        private List<PlayerCore> CreatePlayers()
        {
            return Enumerable.Range(0, _gridSize.x)
                .SelectMany(x => {
                    return Enumerable.Range(0, _gridSize.y)
                        .Select(y => new Vector2(x, y));
                })
                .Select(grid => {
                    var offset = _gridInterval.X0Y();
                    offset.Scale(grid.X0Y());
                    var position = _firstGridPosition + offset;
                    return CreatePlayer(position, Quaternion.identity);
                })
                .ToList();
        }

        private PlayerCore CreatePlayer(Vector3 position, Quaternion rotation)
        {
            int i = Random.Range(0, machines.Count);
            var player = Instantiate(machines[i], position, rotation);

            return player.GetComponent<PlayerCore>();
        }
    }
}