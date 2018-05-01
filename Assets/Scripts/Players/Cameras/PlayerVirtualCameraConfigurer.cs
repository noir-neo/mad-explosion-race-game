using Cinemachine;
using Players;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class PlayerVirtualCameraConfigurer : MonoBehaviour
{
    [SerializeField] private PlayerCore _playerCore;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    void Start()
    {
        _virtualCamera.enabled = false;
        _playerCore.PlayerEntryNumber
            .Subscribe(Configure)
            .AddTo(this);
    }

    private void Configure(int playerNumber)
    {
        if (0 < playerNumber)
        {
            _virtualCamera.enabled = true;
            gameObject.layer = LayerMask.NameToLayer($"VCamPlayer{playerNumber}");
        }
    }
}
