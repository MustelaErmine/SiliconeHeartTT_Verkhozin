using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller<BootstrapInstaller>
{
    [SerializeField] string nextScene;
    public override void InstallBindings()
    {
        Container.Bind<GoImmediately.Settings>().FromInstance(new GoImmediately.Settings() { nextScene = nextScene });
        Container.BindInterfacesTo<GoImmediately>().AsSingle().NonLazy();
    }
}