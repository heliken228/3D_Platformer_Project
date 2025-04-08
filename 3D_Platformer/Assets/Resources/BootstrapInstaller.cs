using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    public GameObject UI_LoadScene;
    public GameObject UI_MainMenu;
    public GameObject UI_Pause;
    public GameObject BackgroundMusic;
    public GameObject UI_GameOver;
    public GameObject UI_About;
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
        
        Container.Bind<UIPause>()
            .FromComponentInNewPrefab(UI_Pause)
            .AsSingle()
            .Lazy();

        Container.Bind<BackgroundMusic>()
            .FromComponentInNewPrefab(BackgroundMusic)
            .AsSingle()
            .Lazy();
        
        Container.Bind<UIGameOver>()
            .FromComponentInNewPrefab(UI_GameOver)
            .AsSingle()
            .Lazy();
        Container.Bind<UIAbout>()
            .FromComponentInNewPrefab(UI_About)
            .AsSingle()
            .Lazy();
    }
}
