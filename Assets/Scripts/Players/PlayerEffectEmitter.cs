using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Players
{
    public class PlayerEffectEmitter : MonoBehaviour
    {
        [SerializeField] private PlayerCore playerCore;
        [SerializeField] private GameObject explosionPrefab;
        private ParticleSystem explosion;

        void Start()
        {
            explosion = Instantiate(explosionPrefab, transform).GetComponent<ParticleSystem>();

            playerCore.Explode
                .Subscribe(_ => explosion.Play())
                .AddTo(this);
        }
    }
}