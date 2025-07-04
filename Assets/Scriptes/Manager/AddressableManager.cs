using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;

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

        // group/label별로 AddressableData를 분류하여 캐싱
        foreach (var data in addressableDataList)
        {
            string key = data.key;
            // 기본적으로 key로 프리팹 로드
            var handle = Addressables.LoadAssetAsync<GameObject>(key);
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                prefabCache[key] = handle.Result;
                Debug.LogError($"[AddressableManager] Loaded prefab: {handle.Result.name} (Group: {data.groupName}, Label: {data.label})");
            }
            else
            {
                Debug.LogError($"[AddressableManager] Failed to load: {key}");
            }
        }
    }

    /// <summary>
    /// key로 프리팹 반환
    /// </summary>
    public GameObject GetCachedPrefab(string _key)
    {
        if (prefabCache.TryGetValue(_key, out var prefab))
        {
            Debug.LogError($"[AddressableManager] Success Prefab not cached: {_key}");
            return prefab;
        }
        Debug.LogError($"[AddressableManager] Prefab not cached: {_key}");
        return null;
    }

    /// <summary>
    /// PoolingType, idx로 key 반환
    /// </summary>
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

    /// <summary>
    /// groupName, label로 AddressableData 리스트 반환
    /// </summary>
    public List<AddressableData> GetAddressableDataByGroupAndLabel(string _groupName, string _label)
    {
        return addressableDataList.Where(x => x.groupName == _groupName && x.label == _label).ToList();
    }

    /// <summary>
    /// label로 Addressable 프리팹들 비동기 로드 (예: "hero", "enemy", "land")
    /// </summary>
    public IEnumerator LoadPrefabsByLabel(string _label)
    {
        var handle = Addressables.LoadAssetsAsync<GameObject>(_label, null);
        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            foreach (var prefab in handle.Result)
            {
                if (!prefabCache.ContainsKey(prefab.name))
                    prefabCache[prefab.name] = prefab;
                Debug.LogError($"[AddressableManager] Loaded by label: {prefab.name}");
            }
        }
        else
        {
            Debug.LogError($"[AddressableManager] Failed to load prefabs by label: {_label}");
        }
    }

    /// <summary>
    /// groupName, label로 Addressable 프리팹들 비동기 로드
    /// </summary>
    public IEnumerator LoadPrefabsByGroupAndLabel(string _groupName, string _label)
    {
        var datas = GetAddressableDataByGroupAndLabel(_groupName, _label);
        foreach (var data in datas)
        {
            if (!prefabCache.ContainsKey(data.key))
            {
                var handle = Addressables.LoadAssetAsync<GameObject>(data.key);
                yield return handle;
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    prefabCache[data.key] = handle.Result;
                    Debug.LogError($"[AddressableManager] Loaded prefab: {handle.Result.name} (Group: {_groupName}, Label: {_label})");
                }
                else
                {
                    Debug.LogError($"[AddressableManager] Failed to load: {data.key}");
                }
            }
        }
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
