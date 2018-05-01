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

            this.FixedUpdateAsObservable()
                .Subscribe(_ => {
                    var ray = new Ray(transform.position, -transform.up);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, _playerCore.HoverHeight)) {
                        var propotionalHeight = (_playerCore.HoverHeight - hit.distance) / _playerCore.HoverHeight;
                        var adjustedHoverForce = Vector3.up * propotionalHeight * _playerCore.HoverPower;
                        _rigidBody.AddForce(adjustedHoverForce, ForceMode.Acceleration);
                    }
                })
                .AddTo(this);
        }
    }
}