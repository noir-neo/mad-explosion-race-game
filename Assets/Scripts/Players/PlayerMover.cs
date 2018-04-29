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
            _playerCore.MovementForceAsObservable()
                .CombineLatest(this.FixedUpdateAsObservable(), (force, _) => force)
                .Subscribe(force => _rigidBody.AddRelativeForce(force * Time.deltaTime))
                .AddTo(this);

            _playerCore.MovementTorqueAsObservable()
                .CombineLatest(this.FixedUpdateAsObservable(), (torque, _) => torque)
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