using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/ResDBRegistry")]
public class ResDBRegistry : ScriptableObject
{
    [System.Serializable]
    public class Entry
    {
        public PoolingType type;
        public ScriptableObject resDB; // IResDB 구현체만 넣기
    }

    public List<Entry> entries;

    private Dictionary<PoolingType, IResDB> resDBMap;

    public void Initialize()
    {
        resDBMap = new Dictionary<PoolingType, IResDB>();
        foreach (var entry in entries)
        {
            if (entry.resDB is IResDB db)
            {
                db.Initialize();
                resDBMap[entry.type] = db;
            }
        }
    }

    public GameObject GetResObj(PoolingType _type, int _idx)
    {
        if (resDBMap == null)
            Initialize();

        if (resDBMap.TryGetValue(_type, out var db))
        {
            return db.GetPrefab(_idx);
        }

        Debug.LogWarning($"No ResDB registered for type {_idx}");
        return null;
    }
}
