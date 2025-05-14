using UnityEngine;

public class BaseUI : MonoBehaviour
{
    [SerializeField]
    protected UIData data;
    public UIData GetUIData() => data;

    public virtual void ShowUI(UIData _data)
    {
        data = _data;
    }
    public virtual void MoveUI()
    {

    }
    public virtual void HideUI() 
    { 

    }
}

