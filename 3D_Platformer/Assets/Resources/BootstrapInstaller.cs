using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    public GameObject UI_LoadScene;
    public override void InstallBindings()
    {
        
        Container.Bind<BonusService>().AsSingle();
       // Container.Bind<Canvas>().FromComponentInNewPrefab(UI_LoadScene).AsSingle().NonLazy();
        //CreateUIPanel();
        
    }


    private void CreateUIPanel()
    {
        GameObject uiPanel = Instantiate(UI_LoadScene);
        Container.InjectGameObject(uiPanel);
        Canvas canvas = uiPanel.GetComponent<Canvas>();
        
    }
}
