using UniRx;

public interface IWeatherUsecase
{
    IReadOnlyReactiveProperty<ApiResult<GetCurrentWeatherRes>> GetCurrentWeatherRes { get; }

    public void GetCurrentWeatherReq(GetCurrentWeatherReq req);
}
