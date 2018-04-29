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
    }
}