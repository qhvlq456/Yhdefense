using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Game/UIResData")]
public class UIResData : ScriptableObject
{
    public UIType uiType;
    public UIPanelType uiPanelType;
    public GameObject prefab;
}
