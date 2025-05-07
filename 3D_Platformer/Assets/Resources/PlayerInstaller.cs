using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    public Transform StartPoint;
    public GameObject PlayerPrefab;
    public Camera MainCamera;
    public SplineCamera SplineCamera;
    public GloveKick GloveKick;

    public override void InstallBindings()
    {
        BindCameras();
        BindPlayerController();
    }
    
    private void BindPlayerController()
    {
        PlayerController playerController =
            Container.InstantiatePrefabForComponent<PlayerController>(PlayerPrefab, StartPoint.position,
                Quaternion.identity, null);

        Container
            .Bind<PlayerController>()
            .FromInstance(playerController)
            .AsSingle();
    }

    private void BindCameras()
    {
        Container.Bind<Camera>().FromInstance(MainCamera).AsSingle();
        Container.Bind<SplineCamera>().FromInstance(SplineCamera).AsSingle();
    }
}
