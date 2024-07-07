using Zenject;

public class WeatherUsecaseInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        AApiResource api = new AApiResource();
        var usecase = new WeatherUsecase(api);
        Container.Bind<IWeatherUsecase>().FromInstance(usecase);
    }
}