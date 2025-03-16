using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<BonusService>().AsSingle();
    }
}
