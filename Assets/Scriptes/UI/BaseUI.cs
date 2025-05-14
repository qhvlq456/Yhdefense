using UnityEngine;

public class BaseUI : MonoBehaviour
{
    public UIData data;

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

