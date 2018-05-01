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
            return _raceStarter.StartRaceAsObservable().Zip(_inputEventProvider, (_, x) => x)
                .First()
                .Select(inputEventProvider => inputEventProvider.GetAccelAsObservable())
                .Switch()
                .Select(accel => Vector3.forward * (accel ? _playerParameters.AccelPower : 0));
        }

        public IObservable<Vector3> MovementTorqueAsObservable()
        {
            return _raceStarter.StartRaceAsObservable().Zip(_inputEventProvider, (_, x) => x)
                .First()
                .Select(inputEventProvider => inputEventProvider.GetSteeringAsObservable())
                .Switch()
                .Select(steering => Vector3.up * _playerParameters.TurnPower * steering);
        }
    }
}