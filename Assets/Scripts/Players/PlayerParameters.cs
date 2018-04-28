using UnityEngine;

namespace Players
{
    [CreateAssetMenu]
    class PlayerParameters : ScriptableObject
    {
        [SerializeField] private float _thrust;
        public float Thrust => _thrust;
    }
}