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

        private readonly ISubject<Unit> _explode = new AsyncSubject<Unit>();
        public IObservable<Unit> Explode => _explode;

        [Inject] private IRaceStarter _raceStarter;
        [Inject] private IRaceTerminator _raceTerminator;

        void Start()
        {
            _raceTerminator.TerminateRaceAsObservable()
                .Subscribe(_ => {
                    _explode.OnNext(Unit.Default);
                    _explode.OnCompleted();
                })
                .AddTo(this);
        }

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
                .TakeUntil(_raceTerminator.TerminateRaceAsObservable())
                .Select(accel => Vector3.forward * (accel ? _playerParameters.AccelPower : 0));
        }

        public IObservable<Vector3> MovementTorqueAsObservable()
        {
            return _raceStarter.StartRaceAsObservable().Zip(_inputEventProvider, (_, x) => x)
                .First()
                .Select(inputEventProvider => inputEventProvider.GetSteeringAsObservable())
                .Switch()
                .TakeUntil(_raceTerminator.TerminateRaceAsObservable())
                .Select(steering => Vector3.up * _playerParameters.TurnPower * steering);
        }
    }
}