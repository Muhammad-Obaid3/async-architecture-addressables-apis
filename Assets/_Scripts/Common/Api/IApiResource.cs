using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Represents an API endpoint
/// </summary>
public interface IApiResource
{
    /// <summary>
    /// The URL endpoint of the resource
    /// </summary>
    string Endpoint { get; set; }

    /// <summary>
    /// The full constructed URL to the endpoint
    /// </summary>
    string Url { get; }

    /// <summary>
    /// The endpoint is constructed by passing a <see cref="StringBuilder"/> up
    /// the tree until the parent is reached. The parent then adds it's portion
    /// of the endpoint. The first child contributes it's portion and so on.
    /// Until the whole endpoint is created. 
    /// </summary>
    void ConstructEndpoint(StringBuilder sb);
    void SetEndpoint(string url);

    Task<ApiResult<TResponse>> GetAsync<TResponse>() where TResponse : AModelV1, new();

    Task<ApiResult> DeleteAsync();

    Task<ApiResult<TResponse>> PostAsync<TRequest, TResponse>(TRequest request)
        where TRequest : AModelV1, new()
        where TResponse : AModelV1, new();

    Task<ApiResult<TResponse>> DownloadAsync<TRequest, TResponse>(TRequest request)
        where TRequest : DownloadReq, new()
        where TResponse : AModelV1, new();

    Task<ApiResult<TResponse>> PostImageAsync<TResponse>(List<IMultipartFormSection> formData)
        where TResponse : AModelV1, new();
}
