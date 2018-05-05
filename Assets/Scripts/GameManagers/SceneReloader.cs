using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using Zenject;

namespace GameManagers
{
    class SceneReloader : MonoBehaviour
    {
        [Inject] private IRaceTerminator raceTerminator;

        void Start()
        {
            raceTerminator.TerminateRaceAsObservable()
                .Delay(TimeSpan.FromSeconds(10))
                .Subscribe(_ => SceneManager.LoadScene("main"))
                .AddTo(this);
        }
    }
}