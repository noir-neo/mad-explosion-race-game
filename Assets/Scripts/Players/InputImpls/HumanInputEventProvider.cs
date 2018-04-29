using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Players
{
    class HumanInputEventProvider : MonoBehaviour, IInputEventProvider
    {
        [SerializeField] private PlayerId _playerId;

        public IObservable<bool> GetAccelAsObservable()
        {
            return this.ObserveEveryValueChanged(_ => Input.GetButton($"Player{(int)_playerId}_Accel"));
        }

        public IObservable<float> GetSteeringAsObservable()
        {
            return this.ObserveEveryValueChanged(_ => Input.GetAxis($"Player{(int)_playerId}_Steering"));
        }
    }
}