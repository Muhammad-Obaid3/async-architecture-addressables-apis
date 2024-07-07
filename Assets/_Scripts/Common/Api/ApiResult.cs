using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// A request of an api call
/// </summary>
public class ApiResult
{
    /// <summary>
    /// True if the request status is a success code
    /// </summary>
    public bool IsSuccess;

    /// <summary>
    /// Error message and custom status code
    /// </summary>
    public Error Error;

    /// <summary>
    /// The completed <see cref="UnityWebRequest"/>
    /// </summary>
    public UnityWebRequest HttpResponse;
}

/// <summary>
/// A result of an api call
/// </summary>
/// <typeparam name="TResult">The type of result expected from the api call</typeparam>
public class ApiResult<TResult>: ApiResult
{
    /// <summary>
    /// The deserialized response from the call. Null if no response received or call unsuccessful.
    /// </summary>
    public TResult Result;
}

/// <summary>
/// A fail result of an api call
/// <typeparam name="Error"> The type of error expected from the api call</typeparam>
/// </summary>
public class Error : AModelV1
{
    public string error;
    public string GetErrorMessage()
    {
        switch (statusCode)
        {
            case Constants.API_KEY_NOT_PROVIDED:
                return Constants.ERROR_API_KEY_NOT_PROVIDED;
        }
        return null;
    }

    public void Popup()
    {
        switch (statusCode)
        {
            default:
                break;
        }
    }
}