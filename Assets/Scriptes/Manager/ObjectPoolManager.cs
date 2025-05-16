using System;
using System.Collections.Generic;
using UnityEngine;

public class Pooling
{
    public Transform root; // PoolingType 기준 루트
    public Dictionary<int, Transform> poolingdic; // idx 별 Transform 그룹

    public Pooling(PoolingType _type, Transform _parent)
    {
        root = new GameObject(_type.ToString()).transform;
        root.position = Vector3.left * 1000;
        poolingdic = new Dictionary<int, Transform>();
        root.SetParent(_parent);
    }

    public Transform Create(PoolingType _type, int _idx)
    {
        Transform ret = null;
        GameObject res = Utility.GetResObj(_type, _idx);
        Transform parent = null;

        if (!poolingdic.TryGetValue(_idx, out parent))
        {
            parent = new GameObject($"Idx_{_idx}").transform;
            parent.SetParent(root);
            parent.localPosition = Vector3.zero;
            poolingdic.Add(_idx, parent);
        }

        if (parent.childCount > 0)
        {
            ret = parent.GetChild(0);
            ret.SetParent(null);
        }
        else
        {
            ret = GameObject.Instantiate(res).transform;
        }

        ret.gameObject.SetActive(true);
        return ret;
    }

    public void Retrieve(PoolingType _type, int _idx, Transform _trf)
    {
        if (poolingdic.TryGetValue(_idx, out Transform parent))
        {
            _trf.SetParent(parent);
        }
        else
        {
            parent = new GameObject($"Idx_{_idx}").transform;
            parent.SetParent(root);
            poolingdic.Add(_idx, parent);
            _trf.SetParent(parent);
        }

        _trf.localPosition = Vector3.zero;
        _trf.localRotation = Quaternion.identity;
        _trf.gameObject.SetActive(false);
    }
}


public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    [SerializeField]
    private Transform root;
    private Dictionary<PoolingType, Pooling> objectPoolingDic = new Dictionary<PoolingType, Pooling>();

    public Transform Create(PoolingType _type, int _idx)
    {
        if (!objectPoolingDic.TryGetValue(_type, out var pool))
        {
            pool = new Pooling(_type, root);
            objectPoolingDic.Add(_type, pool);
        }

        return pool.Create(_type, _idx);
    }

    public void Retrieve(PoolingType _type, int _idx, Transform _trf)
    {
        if (objectPoolingDic.TryGetValue(_type, out var pool))
        {
            pool.Retrieve(_type, _idx, _trf);
        }
        else
        {
            Debug.LogWarning($"Trying to return object to unknown pool: {_type}-{_idx}");
        }
    }
}

