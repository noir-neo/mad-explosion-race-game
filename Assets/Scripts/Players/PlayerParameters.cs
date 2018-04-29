using UnityEngine;

namespace Players
{
    [CreateAssetMenu]
    class PlayerParameters : ScriptableObject
    {
        [SerializeField] private float _accelPower;
        public float AccelPower => _accelPower;

        [SerializeField] private float _turnPower;
        public float TurnPower => _turnPower;

        [SerializeField] private float _hoverHeight;
        public float HoverHeight => _hoverHeight;

        [SerializeField] private float _hoverPower;
        public float HoverPower => _hoverPower;
    }
}