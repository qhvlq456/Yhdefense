using System.Collections.Generic;
using UnityEngine;

public abstract class GenericResDB<T> : ScriptableObject, IResDB where T : BaseResData
{
    [SerializeField]
    protected List<T> resList;

    protected Dictionary<int, GameObject> resDic;

    public virtual void Initialize()
    {
        resDic = new Dictionary<int, GameObject>();
        foreach (var data in resList)
        {
            if (!resDic.ContainsKey(data.index))
                resDic.Add(data.index, data.prefab);
        }
    }

    public virtual GameObject GetPrefab(int index)
    {
        if (resDic == null)
            Initialize();

        if (resDic.TryGetValue(index, out var prefab))
            return prefab;

        Debug.LogWarning($"Prefab not found for index {index}");
        return null;
    }
}
