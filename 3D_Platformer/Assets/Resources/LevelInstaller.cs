using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    public Transform StartPoint;
    public GameObject PlayerPrefab;
    public GameObject Camera;

    public override void InstallBindings()
    {
        BindPlayerMovement();
        BindPlayerInput();
        Container.Bind<GameObject>().FromInstance(Camera).AsSingle();
        Container.BindInstance(PlayerPrefab).WhenInjectedInto<CharacterMovement>();
    }

    private void BindPlayerInput()
    {
        Container.Bind<PlayerInput>().FromInstance(PlayerPrefab.GetComponent<PlayerInput>()).AsSingle();
        Container.BindInstance(PlayerPrefab).WhenInjectedInto<CharacterMovement>();
    }

    private void BindPlayerMovement()
    {
        CharacterMovement characterMovement =
            Container.InstantiatePrefabForComponent<CharacterMovement>(PlayerPrefab, StartPoint.position,
                Quaternion.identity, null);

        Container
            .Bind<CharacterMovement>()
            .FromInstance(characterMovement)
            .AsSingle();

        CinemachineCamera cinemachineCamera = Camera.GetComponent<CinemachineCamera>();
        if (cinemachineCamera != null)
        {
            cinemachineCamera.Follow = characterMovement.transform;
        }
        else
        {
            Debug.LogError("CinemachineVirtualCamera component not found on the camera.");
        }
    }
    
    
    
}
