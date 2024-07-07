using UnityEngine;
using Zenject;
using UniRx;
using UnityEngine.UI;
using ModestTree;

public class WeatherView : MonoBehaviour
{
    //Injection
    [Inject]
    public IWeatherUsecase _weatherUsecase;
    [Inject]
    public DemoPrefab.Factory _demoPrefabFactory;
    [Inject]
    public ISignals _signals;


    //Buttons
    [SerializeField] private Button _btnSpawnPrefab;
    [SerializeField] private Button _searchBtn;

    //Texts
    [SerializeField] private TMPro.TMP_Text _textWeatherData;

    //InputFields
    [SerializeField] private TMPro.TMP_InputField _inputWeatherData;

    private string _findStr;


    public void Construct(IWeatherUsecase weatherUsecase)
    {
        _weatherUsecase = weatherUsecase;
    }

    // Start is called before the first frame update
    void Start()
    {
        _weatherUsecase.GetCurrentWeatherRes.Subscribe((x) => { OnGetCurrentWeatherRes(x); }).AddTo(this);

        _signals.EnterStateSignal.Subscribe((x) => { OnEnterStateSignal(x); }).AddTo(this);

        if (_textWeatherData != null)
            _inputWeatherData.onEndEdit.AddListener(OnInputFieldEndEdit);

        if (_btnSpawnPrefab != null)
            _btnSpawnPrefab.onClick.AddListener(delegate { OnClickSpawnPrefab(); });

        if (_searchBtn != null)
            _searchBtn.onClick.AddListener(delegate { OnFindWeather(); });

        string country = "Pakistan";
        GetCurrentWeather(country);
        _inputWeatherData.SetTextWithoutNotify(country);
    }

    // Callback function for the onEndEdit event
    void OnInputFieldEndEdit(string text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        if (!_inputWeatherData.text.IsEmpty())
        {
            _findStr = text;
        }
    }

    private void OnFindWeather()
    {
        if (string.IsNullOrEmpty(_findStr))
            return;

        GetCurrentWeather(_findStr);

    }

    private void OnEnterStateSignal(EnterStateSignal signal)
    {
        if (signal == null)
        {
            return;
        }
        Debug.Log("enter state signal is called successfully!");
    }

    private void OnClickChangePanel()
    {
        NotifyEnterState();
    }

    private void NotifyEnterState()
    {
        EnterStateSignal signal = new EnterStateSignal()
        {

        };
        _signals.NotifyEnterStateSignal(signal);
    }
    private void OnClickSpawnPrefab()
    {
        SpawnPrefab();
    }

    private void SpawnPrefab()
    {
        DemoPrefab prefab = _demoPrefabFactory.Create();
        prefab.SetPosition(Vector3.zero);
        prefab.SetRotation(Quaternion.identity);
    }

    private void GetCurrentWeather(string cName)
    {
        var req = new GetCurrentWeatherReq()
        {
            apiKey = "a261c7bd7c0c457cad464247240707",
            querryParameter = cName //"Pakistan"
        };
        _weatherUsecase.GetCurrentWeatherReq(req);
    }

    private void OnGetCurrentWeatherRes(ApiResult<GetCurrentWeatherRes> result)
    {
        if (result == null)
        {
            return;
        }
        if (result.IsSuccess)
        {
            Current current = result.Result.current;
            Location location = result.Result.location;



            _textWeatherData.text = $"Country: {location.country}\nRegion: {location.region}\nLattitude:{location.lat}\nLonitude: {location.lon}\n" +
                $"Local Time: {location.localtime}\nTime Zone: {location.tz_id}\nHumidity: {current.humidity}\ntemp_c: {current.temp_c}\ntemp_f: {current.temp_f}";
        }
        else
        {
            var str = result.Error.GetErrorMessage();
            Debug.LogError(str);
        }
    }
}
