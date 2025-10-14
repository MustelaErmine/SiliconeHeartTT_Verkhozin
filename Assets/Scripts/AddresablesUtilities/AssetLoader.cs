using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetLoader
{
    AsyncOperationHandle _operationHandle;
    public IEnumerator GetAsset<T>(string path, Action<T> callback = null) where T : UnityEngine.Object
    {
        if (_operationHandle.IsValid())
        {
            throw new NotImplementedException();
        }

        _operationHandle = Addressables.LoadAssetAsync<T>(path);
        yield return _operationHandle;

        if (!_operationHandle.IsValid())
        {
            throw new NotImplementedException();
        }

        var asset = _operationHandle.Result as T;

        callback?.Invoke(asset);
    }
    public void Release()
    {
        Addressables.Release(_operationHandle);
    }
}

