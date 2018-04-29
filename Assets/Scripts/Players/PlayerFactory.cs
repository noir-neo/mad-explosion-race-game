using System.Collections.Generic;
using UnityEngine;

namespace Players
{
    class PlayerFactory : MonoBehaviour
    {
        [SerializeField] private List<GameObject> machines;

        void Start()
        {
            int i = Random.Range(0, machines.Count);
            var player = Instantiate(machines[i]);
            
            var playerCore = player.GetComponent<PlayerCore>();

            var inputEventProvider = player.AddComponent<HumanInputEventProvider>();
            inputEventProvider.Inject(PlayerId.Player1);

            playerCore.Inject(inputEventProvider);
        }
    }
}