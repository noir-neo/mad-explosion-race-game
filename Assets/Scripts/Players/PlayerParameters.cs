using UnityEngine;

namespace Players
{
    class PlayerParameters : MonoBehaviour
    {
        [SerializeField] private float _thrust;
        public float Thrust => _thrust;
    }
}