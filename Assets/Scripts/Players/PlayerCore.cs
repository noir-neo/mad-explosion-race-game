using System;
using UniRx;
using UnityEngine;

namespace Players
{
    class PlayerCore : MonoBehaviour
    {
        [SerializeField] private PlayerParameters _playerParameters;

        private IInputEventProvider _inputEventProvider;

        public void Inject(IInputEventProvider inputEventProvider)
        {
            _inputEventProvider = inputEventProvider;
        }

        private IObservable<IInputEventProvider> InputEventProviderAsObservable()
        {
            return this.ObserveEveryValueChanged(_ => _inputEventProvider)
                .Where(v => v != null);
        }

        public IObservable<Vector3> MovementForceAsObservable()
        {
            return InputEventProviderAsObservable()
                .SelectMany(inputEventProvider =>
                {
                    return inputEventProvider
                        .GetAccelAsObservable()
                        .Select(accel => Vector3.forward * (accel ? _playerParameters.AccelPower : 0));
                });
        }

        public IObservable<Vector3> MovementTorqueAsObservable()
        {
            return InputEventProviderAsObservable()
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