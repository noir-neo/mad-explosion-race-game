using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Players
{
    class PlayerMover : MonoBehaviour
    {
        [SerializeField] private PlayerCore _playerCore;
        [SerializeField] private Rigidbody _rigidBody;

        void Start()
        {
            this.FixedUpdateAsObservable()
                .WithLatestFrom(_playerCore.MovementForceAsObservable(), (_, x) => x)
                .Subscribe(force => _rigidBody.AddRelativeForce(force * Time.deltaTime))
                .AddTo(this);

            this.FixedUpdateAsObservable()
                .WithLatestFrom(_playerCore.MovementTorqueAsObservable(), (_, x) => x)
                .Select(torque => Quaternion.Euler(torque * Time.deltaTime))
                .Subscribe(delta => _rigidBody.MoveRotation(_rigidBody.rotation * delta))
                .AddTo(this);
        }
    }
}