using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class CameraTargetAssigner : MonoBehaviour
{
    [SerializeField] private CinemachineCamera _freeLookCamera;

    [Inject]
    private void Construct(PlayerController player)
    {
        Transform cameraTarget = player.CameraTarget;
        _freeLookCamera.Follow = cameraTarget;
        _freeLookCamera.LookAt = cameraTarget;
    }
}
