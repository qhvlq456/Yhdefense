using UnityEngine;

[CreateAssetMenu(menuName = "Game/UI/Resource", fileName = "UIData")]
public class UIData : ScriptableObject
{
    public UIPanelType panelType;
    public UIType canvasType;
    public GameObject prefab;
    public bool usePooling;
}
