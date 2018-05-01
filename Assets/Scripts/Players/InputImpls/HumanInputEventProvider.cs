using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Players.InputImpls
{
    class HumanInputEventProvider : MonoBehaviour, IInputEventProvider
    {
        private readonly ISubject<int> _playerId = new AsyncSubject<int>();

        public void Inject(int playerId)
        {
            _playerId.OnNext(playerId);
            _playerId.OnCompleted();
        }

        public IObservable<bool> GetAccelAsObservable()
        {
            return _playerId.Select(id => this.ObserveEveryValueChanged(_ => Input.GetButton($"Player{id}_Accel")))
                .Switch();
        }

        public IObservable<float> GetSteeringAsObservable()
        {
            return _playerId.Select(id => this.ObserveEveryValueChanged(_ => Input.GetAxis($"Player{id}_Steering")))
                .Switch();
        }
    }
}