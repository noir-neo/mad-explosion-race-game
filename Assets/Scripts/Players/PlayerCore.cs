using System;
using Cinemachine;
using UniRx;
using UnityEngine;
using Zenject;
using GameManagers;

namespace Players
{
    class PlayerCore : MonoBehaviour
    {
        [SerializeField] private PlayerParameters _playerParameters;
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

        private IInputEventProvider _inputEventProvider;

        [Inject] private IRaceStarter _raceStarter;

        public void Configure(IInputEventProvider inputEventProvider, bool enableCamera = false)
        {
            _inputEventProvider = inputEventProvider;
            _cinemachineVirtualCamera.enabled = enableCamera;
        }

        private IObservable<IInputEventProvider> InputEventProviderAsObservable()
        {
            return this.ObserveEveryValueChanged(_ => _inputEventProvider)
                .Where(v => v != null);
        }

        public IObservable<Vector3> MovementForceAsObservable()
        {
            return _raceStarter.StartRaceAsObservable()
                .SelectMany(_ => InputEventProviderAsObservable())
                .SelectMany(inputEventProvider =>
                {
                    return inputEventProvider
                        .GetAccelAsObservable()
                        .Select(accel => Vector3.forward * (accel ? _playerParameters.AccelPower : 0));
                });
        }

        public IObservable<Vector3> MovementTorqueAsObservable()
        {
            return _raceStarter.StartRaceAsObservable()
                .SelectMany(_ => InputEventProviderAsObservable())
                .SelectMany(inputEventProvider =>
                {
                    return inputEventProvider
                        .GetSteeringAsObservable()
                        .Select(steering => Vector3.up * _playerParameters.TurnPower * steering);
                });
        }

        public float HoverHeight => _playerParameters.HoverHeight;

        public float HoverPower => _playerParameters.HoverPower;
    }
}