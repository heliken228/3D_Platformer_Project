using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    public GameObject UI_LoadScene;
    public GameObject UI_MainMenu;
    public override void InstallBindings()
    {
        Container.Bind<BonusService>().AsSingle();

        Container.Bind<UILoadingPanel>()
            .FromComponentInNewPrefab(UI_LoadScene)
            .AsSingle()
            .Lazy();
        
        Container.Bind<UIMainMenu>()
            .FromComponentInNewPrefab(UI_MainMenu)
            .AsSingle()
            .Lazy();
    }
}
