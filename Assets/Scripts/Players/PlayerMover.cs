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
                .WithLatestFrom(_playerCore.MovementForce, (_, force) => force)
                .WithLatestFrom(_playerCore.MovementTorque, (force, torque) => Tuple.Create(force, torque))
                .Subscribe(tuple =>
                {
                    _rigidBody.AddRelativeForce(tuple.Item1 * Time.deltaTime);
                    _rigidBody.AddRelativeTorque(tuple.Item2 * Time.deltaTime);
                })
                .AddTo(this);
        }
    }
}