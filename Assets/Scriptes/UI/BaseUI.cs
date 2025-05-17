using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    public UIPanelType panelType { get; private set; }

    public virtual void Initialize(UIPanelType _panelType)
    {
        panelType = _panelType;
    }

    public virtual void ShowUI() => gameObject.SetActive(true);
    public virtual void HideUI() => gameObject.SetActive(false);
}
