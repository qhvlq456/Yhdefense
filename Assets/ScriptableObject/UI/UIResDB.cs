using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Game/UIResDB")]
public class UIResDB : MonoBehaviour
{
    public List<UIResData> uiList;

    private Dictionary<UIPanelType, GameObject> uiDic;

    public void Initialize()
    {
        uiDic = new Dictionary<UIPanelType, GameObject>();
        foreach (var data in uiList)
        {
            if (!uiDic.ContainsKey(data.uiPanelType))
                uiDic.Add(data.uiPanelType, data.prefab);
        }
    }

    public GameObject GetPrefab(UIPanelType _uiPanelType)
    {
        if (uiDic == null)
        {
            Initialize();
        }

        if (uiDic.TryGetValue(_uiPanelType, out var obj))
            return obj;

        Debug.LogWarning($"Prefab not found for _uiPanelType {_uiPanelType}");
        return null;
    }
}
