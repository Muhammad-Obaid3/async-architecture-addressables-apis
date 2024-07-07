using UnityEngine;
using Zenject;

public class SignalsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.
            Bind<ISignals>().To<Signals>().AsSingle();
    }
}