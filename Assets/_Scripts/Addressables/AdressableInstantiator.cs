using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AdressableInstantiator : MonoBehaviour
{

    [SerializeField] private AssetReferenceGameObject _cube1;


    private GameObject _instanceReference;

    [SerializeField] private TMPro.TMP_Text _loadingText;



    [SerializeField] private Button _loadAssetsBtn;
    [SerializeField] private Button _unLoadAssetsBtn;

    private void Start()
    {
        _loadAssetsBtn.onClick.AddListener(delegate { LoadAssets(); });
        _unLoadAssetsBtn.onClick.AddListener(delegate { UnLoadAssets(); });
    }


    //public void LoadAssets()
    //{
    //    _loadAssetsBtn.interactable = false;
    //    StartCoroutine(LoadAssetWithProgress());
    //}

    public void LoadAssets()
    {
        if (_instanceReference == null && !_cube1.OperationHandle.IsValid())
        {
            _loadAssetsBtn.interactable = false;
            StartCoroutine(LoadAssetWithProgress());
        }
        else
        {
            Debug.Log("Asset is already loaded or in the process of loading.");
        }
    }


    public void UnLoadAssets()
    {
        if (_instanceReference == null)
        {
            return;
        }
        _instanceReference.gameObject.SetActive(false);
        _loadingText.SetText(string.Empty);

        // Assuming _instanceReference is the GameObject instance you want to unload
        if (_instanceReference != null)
        {
            // Release the instance
            _cube1.ReleaseInstance(_instanceReference);
            _instanceReference = null;
        }

        // Unload the asset
        _cube1.ReleaseAsset();

    }

    private IEnumerator LoadAssetWithProgress()
    {
        var handle = _cube1.LoadAssetAsync<GameObject>();

        while (!handle.IsDone)
        {
            float percentComplete = handle.PercentComplete * 100;
            _loadingText.text = $"Loading progress: {percentComplete:F2}%";

            yield return null;
        }

        _loadingText.text = "Loading progress: 100%";

        // Call the completion callback
        OnAddressableInstantiated(handle);
    }

    void OnAddressableInstantiated(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.IsValid())
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _instanceReference = handle.Result;
                _instanceReference = Instantiate(_instanceReference);
                _loadAssetsBtn.interactable = true;
                _instanceReference.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("Failed to instantiate addressable: " + handle.Status);
            }
        }
        else
        {
            Debug.LogError("Invalid operation handle: " + handle);
        }
    }

}
