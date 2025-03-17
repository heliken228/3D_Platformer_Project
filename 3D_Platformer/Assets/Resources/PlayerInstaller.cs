using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    public Transform StartPoint;
    public GameObject PlayerPrefab;

    public override void InstallBindings()
    {
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
}
