using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : Singleton<AddressableManager>
{
    private readonly Dictionary<string, GameObject> prefabCache = new();
    [SerializeField] 
    private List<AddressableData> addressableDataList = new();
    [SerializeField] 
    private List<GameObject> loadedList = new();

    protected override void Awake()
    {
        base.Awake();
        LoadData();
        StartCoroutine(InitAddressable());
    }

    private void LoadData()
    {
        addressableDataList = NewtonSoftJson.LoadJsonArray<AddressableData>(Application.streamingAssetsPath, "AddressableData");
        Debug.LogError(DataLogger.LogList(addressableDataList));
    }

    private IEnumerator InitAddressable()
    {
        yield return Addressables.InitializeAsync();

        foreach (var data in addressableDataList)
        {
            string key = data.key;

            var handle = Addressables.LoadAssetAsync<GameObject>(key);
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                prefabCache[key] = handle.Result;
                Debug.LogError($"[AddressableManager] Loaded prefab: {handle.Result.name}");
            }
            else
            {
                Debug.LogError($"[AddressableManager] Failed to load: {key}");
            }
        }
    }

    public GameObject GetCachedPrefab(string _name)
    {
        if (prefabCache.TryGetValue(_name, out var prefab))
        {
            return prefab;
        }

        Debug.LogError($"[AddressableManager] Prefab not cached: {_name}");
        return null;
    }
    public string GetAddressableName(PoolingType _type, int _idx)
    {
        var data = addressableDataList.Find(x => x.type == _type && x.idx == _idx);
        if (data.Equals(default(AddressableData)))
        {
            Debug.LogError($"[AddressableManager] AddressableData not found for type:{_type}, idx:{_idx}");
            return null;
        }
        return data.key;
    }

    public void ReleaseInstance(GameObject _go)
    {
        if (_go != null)
        {
            Addressables.ReleaseInstance(_go);
            loadedList.Remove(_go);
        }
    }

    public void AllReleaseInstance()
    {
        foreach (var go in loadedList)
        {
            Addressables.ReleaseInstance(go);
        }
        loadedList.Clear();
    }

    public void ReleaseAllCachedPrefabs()
    {
        foreach (var prefab in prefabCache.Values)
        {
            Addressables.Release(prefab);
        }
        prefabCache.Clear();
    }
}
