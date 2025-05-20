using UnityEngine;

public interface IClickable
{
    public void OnClick(); 
}

public interface ISelectable
{
    public void OnSelect();
    public void OnDeselect();
}

public interface IHoverable
{
    public void OnHoverEnter();
    public void OnHoverExit();
}
