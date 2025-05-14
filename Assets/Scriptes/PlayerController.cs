using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputSystem_Actions actions;
    private string previousActionMap = "Player";

    private void Awake()
    {
        actions = new InputSystem_Actions();

        // Player ActionMap 클릭 바인딩
        actions.Player.LeftClick.performed += OnLeftClick;
        actions.Player.RightClick.performed += OnRightClick;

        // UI ActionMap 클릭 바인딩
        actions.UI.Click.performed += OnUIClick;
    }

    private void OnEnable()
    {
        actions.Player.Enable(); // 기본적으로 Player 맵 활성화
    }

    private void OnDisable()
    {
        actions.Player.Disable();
        actions.UI.Disable();
    }

    private void OnLeftClick(InputAction.CallbackContext _ctx)
    {
        Debug.Log("왼쪽 클릭");
    }

    private void OnRightClick(InputAction.CallbackContext _ctx)
    {
        Debug.Log("오른쪽 클릭");
    }

    private void OnUIClick(InputAction.CallbackContext _ctx)
    {
        Debug.Log("UI 클릭");
    }


    /// <summary>
    /// UI가 열릴 때 호출 (외부에서)
    /// </summary>
    public void SwitchToUIInput()
    {
        if (actions.Player.enabled)
        {
            actions.Player.Disable();
            previousActionMap = "Player";
        }

        actions.UI.Enable();
        Debug.Log("Input ActionMap: UI로 전환됨");
    }

    /// <summary>
    /// UI가 닫힐 때 호출 (외부에서)
    /// </summary>
    public void SwitchToPlayerInput()
    {
        if (actions.UI.enabled)
        {
            actions.UI.Disable();
        }

        if (previousActionMap == "Player")
        {
            actions.Player.Enable();
            Debug.Log("Input ActionMap: Player로 복귀됨");
        }
    }
}
