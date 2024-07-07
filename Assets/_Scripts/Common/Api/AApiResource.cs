using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class AApiResource : IApiResource
{

    private StringBuilder _sb = new StringBuilder();
    private StringBuilder _url = new StringBuilder();
    private string _endpoint = "";

    public AApiResource()
    { }

    public string Endpoint
    {
        get
        {
            return _endpoint;
        }
        set
        {
            _endpoint = value;
        }
    }
    public string Url
    {
        get
        {
            _sb.Clear();
            ConstructEndpoint(_sb);
            return _sb.ToString();
        }
    }

    public void SetEndpoint(string url)
    {
        _url.Clear();
        _url.Append(url);
    }

    public void ConstructEndpoint(StringBuilder sb)
    {
        sb.Append(Endpoint);
        sb.Append(_url);
    }

    private void PopulateAuthHeaders(UnityWebRequest client)
    {
        string AuthToken = "";//JWT
        client.SetRequestHeader("Authorization", $"Bearer{AuthToken}");
        client.SetRequestHeader("User-Agent", $"hexthedev/api_unity");
        client.SetRequestHeader("deviceinfo", "{}");
    }
    #region GET

    /// <summary>
    /// implements an async get request
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <returns></returns>
    public async Task<ApiResult<TResponse>> GetAsync<TResponse>()
        where TResponse : AModelV1, new()
    {
        UnityWebRequest response = await GetRequestAsync();
        return await PackResultAsync<TResponse>(response);
    }

    #endregion


    #region DELETE
    public async Task<ApiResult> DeleteAsync()
    {
        UnityWebRequest response = await DeleteRequestAsync();
        return PackResult_ResponseOnly(response);
    }


    #endregion

    #region POST
    public async Task<ApiResult<TResponse>> PostAsync<TRequest, TResponse>(TRequest request)
        where TRequest : AModelV1, new()
        where TResponse : AModelV1, new()
    {
        UnityWebRequest response = await PostRequestAsync(request);
        return await PackResultAsync<TResponse>(response);
    }

    private async Task<UnityWebRequest> PostRequestAsync<TRequest>(TRequest request)
        where TRequest : AModelV1, new()
    {
        UnityWebRequest client = UnityWebRequest.PostWwwForm(Url, string.Empty);
        //PopulateAuthHeaders(client);
        AddJsonToUnityWebRequest(client, JsonUtility.ToJson(request));

        /*await*/
        client.SendWebRequest();
        client.uploadHandler.Dispose();
        return client;
    }
    public async Task<ApiResult<TResponse>> PostImageAsync<TResponse>(List<IMultipartFormSection> formData)
        where TResponse : AModelV1, new()
    {
        UnityWebRequest response = await PostRequestImageAsync(formData);
        return await PackResultAsync<TResponse>(response);
    }


    #endregion

    #region DOWNLOAD
    public async Task<ApiResult<TResponse>> DownloadAsync<TRequest, TResponse>(TRequest request)
        where TRequest : DownloadReq, new()
        where TResponse : AModelV1, new()
    {
        UnityWebRequest response = await DownloadRequestAsync(request);
        return await PackResultAsync<TResponse>(response);
    }
    #endregion

    private async Task<UnityWebRequest> DownloadRequestAsync<TRequest>(TRequest request)
        where TRequest : DownloadReq, new()
    {
        UnityWebRequest client = UnityWebRequest.Get(request.url);
        client.downloadHandler = new DownloadHandlerFile(request.filePath);
        /*await*/
        client.SendWebRequest();
        return client;
    }


    private async Task<UnityWebRequest> PostRequestImageAsync(List<IMultipartFormSection> multipartFormSections)
    {
        UnityWebRequest client = UnityWebRequest.Post(Url, multipartFormSections);
        //PopulateAuthHeaders();
        /*await*/
        client.SendWebRequest();
        client.uploadHandler.Dispose();
        return client;
    }

    private void AddJsonToUnityWebRequest(UnityWebRequest client, string json)
    {
        client.SetRequestHeader("Content-Type", "application/json");
        client.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json)
            );
    }

    private async Task<UnityWebRequest> DeleteRequestAsync()
    {
        UnityWebRequest client = UnityWebRequest.Delete(Url);
        //PopulateAuthHeaders(client);
        /*await*/
        client.SendWebRequest();
        return client;
    }

    private async Task<UnityWebRequest> GetRequestAsync()
    {
        UnityWebRequest client = UnityWebRequest.Get(Url);
        //PopulateAuthHeaders(client);
        /*await*/
        client.SendWebRequest();
        return client;
    }

    private ApiResult PackResult_ResponseOnly(UnityWebRequest response)
    {
        ApiResult result = new ApiResult()
        {
            IsSuccess = response.result == UnityWebRequest.Result.Success,
            HttpResponse = response
        };
        return result;
    }
    //private ApiResult<TResponse> PackResult<TResponse>(UnityWebRequest response)
    //    where TResponse : AModelV1, new()
    //{
    //    ApiResult<TResponse> result = new ApiResult<TResponse>()
    //    {
    //        IsSuccess = response.result == UnityWebRequest.Result.Success,
    //        HttpResponse = response
    //    };

    //    if (result.IsSuccess)
    //    {
    //        string resultAsstring = "";
    //        try
    //        {
    //            resultAsstring = response.downloadHandler?.text;
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.LogError(ex.Message);
    //        }

    //        TResponse obj = JsonConvert.DeserializeObject<TResponse>(resultAsstring);
    //        result.Result = obj;
    //    }
    //    else
    //    {
    //        string resultAsString = response.downloadHandler.text;
    //        result.Error = UnpackResponseObject<Error>(resultAsString);
    //        result.Error.Popup();
    //    }
    //    return result;
    //}

    private TModel UnpackResponseObject<TModel>(string content)
        where TModel : AModelV1, new()
    {
        TModel res = JsonUtility.FromJson<TModel>(content);
        return res;
    }


    /**********************************************************/
    private async Task<ApiResult<TResponse>> PackResultAsync<TResponse>(UnityWebRequest response)
    where TResponse : AModelV1, new()
    {

        // Introduce a delay using Task.Delay
        await DelayAsync(700); // Adjust delay time as needed (1000 ms = 1 second)

        ApiResult<TResponse> result = new ApiResult<TResponse>()
        {
            IsSuccess = response.result == UnityWebRequest.Result.Success,
            HttpResponse = response
        };

     

        if (result.IsSuccess)
        {
            string resultAsString = response.downloadHandler?.text;

            try
            {
                TResponse obj = JsonConvert.DeserializeObject<TResponse>(resultAsString);
                result.Result = obj;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error deserializing response: {ex.Message}");
            }
        }
        else
        {
            string resultAsString = response.downloadHandler.text;
            result.Error = UnpackResponseObject<Error>(resultAsString);
            result.Error.Popup();
        }

        return result;
    }

    // Helper method to delay asynchronously
    private async Task DelayAsync(int millisecondsDelay)
    {
        await Task.Delay(millisecondsDelay);
    }


}

