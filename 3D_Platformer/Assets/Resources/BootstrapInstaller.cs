using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    public GameObject UI_LoadScene;
    public GameObject UI_MainMenu;
    public GameObject UI_Pause;
    public GameObject BackgroundMusic;
    public GameObject UIMusic;
    public GameObject ButtonClickAudio;
    public GameObject BonusCollectEffect;
    public GameObject UI_GameOver;
    public GameObject UI_About;
    public GameObject UI_Settings;
    public GameObject UI_Timer;
    public GameObject UI_GameEnd;
    public override void InstallBindings()
    {
        Container.Bind<BonusService>().AsSingle();

        Container.Bind<SceneService>().AsSingle();

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
        
        Container.Bind<UIMusic>()
            .FromComponentInNewPrefab(UIMusic)
            .AsSingle()
            .Lazy();
        
        Container.Bind<ButtonClickAudio>()
            .FromComponentInNewPrefab(ButtonClickAudio)
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
        
        Container.Bind<UISettings>()
            .FromComponentInNewPrefab(UI_Settings)
            .AsSingle()
            .Lazy();
        
        Container.Bind<BonusEffectProjectContext>()
            .FromComponentInNewPrefab(BonusCollectEffect)
            .AsSingle()
            .Lazy();
        
        Container.Bind<UITimer>()
            .FromComponentInNewPrefab(UI_Timer)
            .AsSingle()
            .Lazy();
        
        Container.Bind<UIGameEnd>()
            .FromComponentInNewPrefab(UI_GameEnd)
            .AsSingle()
            .Lazy();
    }
}
