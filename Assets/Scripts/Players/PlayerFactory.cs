using System.Collections.Generic;
using AIs;
using Cinemachine;
using Players.InputImpls;
using UnityEngine;
using Zenject;

namespace Players
{
    class PlayerFactory : MonoBehaviour
    {
        [SerializeField] private List<GameObject> machines;

        [Inject] private CoursePath coursePath;

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

        private PlayerCore CreatePlayer(Vector3 position, Quaternion rotation)
        {
            int i = Random.Range(0, machines.Count);
            var player = Instantiate(machines[i], position, rotation);

            return player.GetComponent<PlayerCore>();
        }
    }
}