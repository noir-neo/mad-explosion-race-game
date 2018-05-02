using System;
using System.Linq;
using UnityEngine;
using Zenject;
using UniRx;

namespace GameManagers
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Camera[] _cameras;

        [Inject] private IHumanInputProvider _humanInputProvider;

        void Start()
        {
            _humanInputProvider.HumanInputIdsAsObservable()
                .Subscribe(x => Configure(x.Count))
                .AddTo(this);
        }

        private void Configure(int numberOfPlayers)
        {
            var activeCamera = _cameras.Take(numberOfPlayers);
            var inactiveCamera = _cameras.Skip(numberOfPlayers);

            foreach (var t in activeCamera.Select((x, i) => Tuple.Create(x, i)))
            {
                t.Item1.rect = CalcCameraRect(t.Item2, numberOfPlayers);
                t.Item1.gameObject.SetActive(true);
            }

            foreach (var camera in inactiveCamera)
            {
                camera.gameObject.SetActive(false);
            }
        }

        private static Rect CalcCameraRect(int cameraIndex, int totalCamera)
        {
            var halfCameraIndex = (cameraIndex + 1) / 2f;
            return new Rect
            {
                width = 1 / Mathf.Ceil(totalCamera / 2f),
                height = totalCamera < 2 ? 1 : 0.5f,
                x = cameraIndex < 2 ? 0 : 0.5f,
                y = totalCamera < 1 ? 0 : halfCameraIndex - Mathf.Floor(halfCameraIndex)
            };
        }
    }
}
