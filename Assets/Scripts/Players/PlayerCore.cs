using System;
using UniRx;
using UnityEngine;

namespace Players
{
    class PlayerCore : MonoBehaviour
    {
        [SerializeField] private PlayerParameters _playerParameters;

        [SerializeField] private HumanInputEventProvider _inputEventProvider;

        public IObservable<Vector3> MovementForce => _inputEventProvider
            .GetAccelAsObservable()
            .Select(accel => Vector3.forward * (accel ? _playerParameters.AccelPower : 0));

        public IObservable<Vector3> MovementTorque => _inputEventProvider
            .GetSteeringAsObservable()
            .Select(steering => Vector3.up * _playerParameters.TurnPower * steering);
    }
}