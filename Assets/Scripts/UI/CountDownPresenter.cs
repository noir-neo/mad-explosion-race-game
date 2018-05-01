using UnityEngine;
using UniRx;
using Zenject;
using GameManagers;
using TMPro;

class CountDownPresenter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;

    [Inject] ICountDownProvider countDownProvider;

    void Start()
    {
        textMesh.text = "";
        countDownProvider
            .CountDownAsObservable()
            .Subscribe(time =>
            {
                textMesh.text = $"{time}";
            }, () =>
            {
                textMesh.enabled = false;
            })
            .AddTo(this);
    }
}