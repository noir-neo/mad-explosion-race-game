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
                .Subscribe(_ =>
                {
                    _rigidBody.AddRelativeForce(_playerCore.MovementForce * Time.deltaTime, ForceMode.Force);
                })
                .AddTo(this);
        }
    }
}