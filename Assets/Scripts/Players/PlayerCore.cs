using UnityEngine;

namespace Players
{
    class PlayerCore : MonoBehaviour
    {
        [SerializeField] private PlayerParameters _playerParameters;

        public Vector3 MovementForce => Vector3.forward * _playerParameters.Thrust;
    }
}