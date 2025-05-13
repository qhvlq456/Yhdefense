using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/LandResDB")]
public class LandResDB : ScriptableObject
{
    public List<LandResData> landList;

    private Dictionary<int, GameObject> landDic;

    public void Initialize()
    {
        landDic = new Dictionary<int, GameObject>();
        foreach (var data in landList)
        {
            if (!landDic.ContainsKey(data.index))
                landDic.Add(data.index, data.prefab);
        }
    }

    public GameObject GetPrefab(int _idx)
    {
        if (landDic == null)
        {
            Initialize();
        }

        if (landDic.TryGetValue(_idx, out var obj))
            return obj;

        Debug.LogWarning($"Prefab not found for index {_idx}");
        return null;
    }
}
