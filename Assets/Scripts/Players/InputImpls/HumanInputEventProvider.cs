using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Players.InputImpls
{
    class HumanInputEventProvider : MonoBehaviour, IInputEventProvider
    {
        private int _playerId;

        public void Inject(int playerId)
        {
            _playerId = playerId;
        }

        private IObservable<int> PlayerIdAsObservable()
        {
            return this.ObserveEveryValueChanged(_ => _playerId)
                .Where(v => v != 0);
        }

        public IObservable<bool> GetAccelAsObservable()
        {
            return this.ObserveEveryValueChanged(_ => Input.GetButton($"Player{_playerId}_Accel"))
                .SkipUntil(PlayerIdAsObservable());
        }

        public IObservable<float> GetSteeringAsObservable()
        {
            return this.ObserveEveryValueChanged(_ => Input.GetAxis($"Player{_playerId}_Steering"))
                .SkipUntil(PlayerIdAsObservable());
        }
    }
}