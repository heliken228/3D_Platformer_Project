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
    }

    private void BindPlayerInput()    //Этот метод привязывает компонент InputController из PlayerPrefab как единственный экземпляр.
    {
        InputController inputController = PlayerPrefab.GetComponent<InputController>();
        if (inputController != null)
        {
            Container.Bind<InputController>().FromInstance(inputController).AsSingle();
        }
        else
        {
            Debug.LogError("PlayerInput component not found on the PlayerPrefab.");
        }
    }

    private void BindPlayerMovement()   //Этот метод создаёт экземпляр CharacterMovement из PlayerPrefab в позиции StartPoint.
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
