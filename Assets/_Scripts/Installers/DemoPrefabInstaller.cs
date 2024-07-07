using UnityEngine;
using Zenject;

public class DemoPrefabInstaller : MonoInstaller<DemoPrefabInstaller>
{
    [SerializeField] private DemoPrefab _prefab;
    public override void InstallBindings()
    {
        Container.
            BindFactory<DemoPrefab, DemoPrefab.Factory>().FromComponentInNewPrefab(_prefab);

        //AApiResource api = new AApiResource();
        //Container.
        //     Bind<IWeatherUsecase>().To<WeatherUsecase>().AsTransient().WithArguments(api);
        
    }
}