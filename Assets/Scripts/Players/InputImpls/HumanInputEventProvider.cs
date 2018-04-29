using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Players
{
    class HumanInputEventProvider : MonoBehaviour, IInputEventProvider
    {
        private PlayerId _playerId;

        public void Inject(PlayerId playerId)
        {
            _playerId = playerId;
        }

        private IObservable<PlayerId> PlayerIdAsObservable()
        {
            return this.ObserveEveryValueChanged(_ => _playerId)
                .Where(v => v != 0);
        }

        public IObservable<bool> GetAccelAsObservable()
        {
            return this.ObserveEveryValueChanged(_ => Input.GetButton($"Player{(int)_playerId}_Accel"))
                .SkipUntil(PlayerIdAsObservable());
        }

        public IObservable<float> GetSteeringAsObservable()
        {
            return this.ObserveEveryValueChanged(_ => Input.GetAxis($"Player{(int)_playerId}_Steering"))
                .SkipUntil(PlayerIdAsObservable());
        }
    }
}