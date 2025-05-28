using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class OldPlayerController : MonoBehaviour
{
    public enum InputState { none, placement }
    [SerializeField]
    private InputState currentState = InputState.none;
    public InputState CurrentState => currentState;
    [SerializeField]
    private GameObject selectObj = null;
    [SerializeField]
    private Hero heroPreivewObj = null;

    private IHoverable lastHover;

    public void EnterPlacementMode(HeroData _heroData)
    {
        MapManager.Instance.SetColorHeroMap();
        currentState = InputState.placement;
        heroPreivewObj = CharacterManager.Instance.CreateHero(_heroData.index, true);
    }
    public void ExitPlacementMode()
    {
        MapManager.Instance.SetHeroOriginalColor();
        currentState = InputState.none;
        ObjectPoolManager.Instance.Retrieve(PoolingType.hero, heroPreivewObj.HeroData.index, heroPreivewObj.transform);
        heroPreivewObj = null;
    }
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
        if(EventSystem.current.IsPointerOverGameObject())
        {
            // UI 위에 있을 때는 아무것도 하지 않음
            return;
        }

        switch (currentState)
        {
            case InputState.none:
                var clickable = GetRayToComponent<IClickable>();

                if (clickable != null)
                {
                    clickable.OnClick();
                }
                break;
            case InputState.placement:
                var heroLand = GetRayToComponent<HeroLand>();
                if (heroLand != null && heroLand.IsHeroEmpty)
                {
                    Vector2Int normalizedPos = NormalizeMousePosition(heroLand.transform.position);

                    if (MapManager.Instance.IsPossibleSetHero(normalizedPos))
                    {
                        heroLand.SetHero(heroPreivewObj.HeroData.index);
                        ExitPlacementMode();
                    }
                    else
                    {
                        // SetHero 불가능한 위치에 놓으려 할 때의 처리 (예: 색상 변경, 메시지 표시 등)
                        UIManager.Instance.ShowUI<AlertPopupUI>(UIPanelType.AlertPopup).ActivateUI();
                    }
                }
                break;
        }
    }

    private void HandleHover()
    {
        switch (currentState)
        {
            case InputState.none:
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
                break;
            case InputState.placement:
                var heroLand = GetRayToComponent<HeroLand>();
                if (heroLand != null && heroLand.IsHeroEmpty)
                {
                    Vector2Int normalizedPos = NormalizeMousePosition(heroLand.transform.position);
                    // Debug.LogError($"normalizedPos : {normalizedPos}");
                    heroPreivewObj.transform.position = new Vector3(normalizedPos.x, heroLand.SetHeroPosY, normalizedPos.y);
                }
                break;
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

    public Vector2Int NormalizeMousePosition(Vector3 _mouseWorldPosition)
    {
        // x와 z 좌표를 1 단위로 정규화
        int normalizedX = Mathf.RoundToInt(_mouseWorldPosition.x);
        int normalizedZ = Mathf.RoundToInt(_mouseWorldPosition.z);

        return new Vector2Int(normalizedX, normalizedZ);
    }
}
