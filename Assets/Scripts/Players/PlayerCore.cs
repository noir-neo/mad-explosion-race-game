using System;
using UniRx;
using UnityEngine;
using Zenject;
using GameManagers;

namespace Players
{
    class PlayerCore : MonoBehaviour
    {
        [SerializeField] private PlayerParameters _playerParameters;

        private readonly ISubject<IInputEventProvider> _inputEventProvider = new AsyncSubject<IInputEventProvider>();
        private readonly ISubject<int> _playerEntryNumber = new AsyncSubject<int>();
        public ISubject<int> PlayerEntryNumber => _playerEntryNumber;

        [Inject] private IRaceStarter _raceStarter;

        public void Configure(IInputEventProvider inputEventProvider, int playerEntryNumber = 0)
        {
            _inputEventProvider.OnNext(inputEventProvider);
            _inputEventProvider.OnCompleted();
            _playerEntryNumber.OnNext(playerEntryNumber);
            _playerEntryNumber.OnCompleted();
        }

        public IObservable<Vector3> MovementForceAsObservable()
        {
            var accelAsObservable = _inputEventProvider
                .Select(inputEventProvider =>
                {
                    return inputEventProvider
                        .GetAccelAsObservable()
                        .Select(accel => Vector3.forward * (accel ? _playerParameters.AccelPower : 0));
                })
                .Switch();
            return _raceStarter.StartRaceAsObservable()
                .First()
                .WithLatestFrom(accelAsObservable, (_, x) => x)
                .Concat(accelAsObservable);
        }

        public IObservable<Vector3> MovementTorqueAsObservable()
        {

            var steeringAsObservable = _inputEventProvider
                .Select(inputEventProvider =>
                {
                    return inputEventProvider
                        .GetSteeringAsObservable()
                        .Select(steering => Vector3.up * _playerParameters.TurnPower * steering);
                })
                .Switch();
            return _raceStarter.StartRaceAsObservable()
                .First()
                .WithLatestFrom(steeringAsObservable, (_, x) => x)
                .Concat(steeringAsObservable);
        }
    }
}