using System.Linq;
using UnityEngine;

public class OldPlayerController : MonoBehaviour
{
    private GameObject selectObj;
    private IHoverable lastHover;

    private void Update()
    {
        HandleHover();

        if (Input.GetMouseButtonDown(0))
        {
            ButtonDownState();
        }

        if (Input.GetMouseButton(0))
        {
            ButtonIngState();
        }

        if (Input.GetMouseButtonUp(0))
        {
            ButtonUpState();
        }
    }

    private void ButtonDownState()
    {
        var selectable = GetRayToComponent<ISelectable>();
        if (selectable != null)
        {
            selectable.OnSelect();
            selectObj = selectable is Component c ? c.gameObject : null;
        }
    }

    private void ButtonIngState()
    {
        // Drag 등 필요 시 작성
    }

    private void ButtonUpState()
    {
        var clickable = GetRayToComponent<IClickable>();

        if (clickable != null)
        {
            clickable.OnClick();
        }
    }

    private void HandleHover()
    {
        var hover = GetRayToComponent<IHoverable>();

        if (hover != lastHover)
        {
            if (lastHover != null)
            {
                lastHover.OnHoverExit();
            }

            if (hover != null)
            {
                hover.OnHoverEnter();
            }

            lastHover = hover;
        }
    }

    private T GetRayToComponent<T>() where T : class
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity)
                                    .OrderBy(h => h.distance).ToArray();

        foreach (var hit in hits)
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.green);
            T component = hit.collider.GetComponent<T>();
            if (component != null)
            {
                return component;
            }
        }

        return null;
    }
}
