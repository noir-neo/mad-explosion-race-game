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
            _playerCore.MovementForce
                .CombineLatest(this.FixedUpdateAsObservable(), (force, _) => force)
                .Subscribe(force => _rigidBody.AddRelativeForce(force * Time.deltaTime))
                .AddTo(this);

            _playerCore.MovementTorque
                .CombineLatest(this.FixedUpdateAsObservable(), (torque, _) => torque)
                .Subscribe(torque => _rigidBody.AddRelativeTorque(torque * Time.deltaTime))
                .AddTo(this);
        }
    }
}