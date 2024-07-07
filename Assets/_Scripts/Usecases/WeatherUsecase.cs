
using UniRx;
public class WeatherUsecase : IWeatherUsecase
{
    public IReadOnlyReactiveProperty<ApiResult<GetCurrentWeatherRes>> GetCurrentWeatherRes => _getCurrentWeatherRes;
    private readonly ReactiveProperty<ApiResult<GetCurrentWeatherRes>> _getCurrentWeatherRes = new ReactiveProperty<ApiResult<GetCurrentWeatherRes>>() { };

    private readonly IApiResource _api;
    public WeatherUsecase(IApiResource api)
    {
        _api = api;
    }

    public async void GetCurrentWeatherReq(GetCurrentWeatherReq req)
    {
        _api.SetEndpoint(req.ToString());
        _api.Endpoint = Constants.WEATHER_API_URL;
        ApiResult<GetCurrentWeatherRes> result = await _api.GetAsync<GetCurrentWeatherRes>();
        _getCurrentWeatherRes.SetValueAndForceNotify(result);
    }
}
