using UnityEngine;

namespace Players
{
    class PlayerCore : MonoBehaviour
    {
        [SerializeField] private PlayerParameters _playerParameters;

        public Vector3 MovementForce => Vector3.forward * _playerParameters.Thrust;
        public Vector3 MovementTorque => Vector3.up * 1200; // TODO: Remove magic number
    }
}