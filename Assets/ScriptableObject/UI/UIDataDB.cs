using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Game/UI/Database", fileName = "UIDataDB")]
public class UIDataDB : ScriptableObject
{
    public List<UIData> uiResources;

    private Dictionary<UIPanelType, UIData> dataDic;

    public void Initialize()
    {
        if (dataDic != null) return;
        dataDic = new();

        foreach (var data in uiResources)
        {
            if (!dataDic.ContainsKey(data.panelType))
                dataDic[data.panelType] = data;
        }
    }

    public UIData Get(UIPanelType _panelType)
    {
        Initialize();
        return dataDic.TryGetValue(_panelType, out var data) ? data : null;
    }
}
