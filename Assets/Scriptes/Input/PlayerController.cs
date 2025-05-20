using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputSystem_Actions actions;
    private string currentActionMap = "Player";

    private void Awake()
    {
        actions = new InputSystem_Actions();
        // Player 맵 바인딩
        actions.Player.LeftClick.performed += OnLeftClick;
        actions.Player.RightClick.performed += OnRightClick;

        // UI 맵 바인딩
        actions.UI.Click.performed += OnUIClick;
    }

    private void OnEnable()
    {
        EnableActionMap("Player"); // 기본 상태는 Player
    }

    private void OnDisable()
    {
        DisableAllMaps();
    }

    // ------------------------
    // 액션 처리
    // ------------------------

    private void OnLeftClick(InputAction.CallbackContext _ctx)
    {
        Debug.Log("Player: 왼쪽 클릭 처리");
        // 게임 상 오브젝트 클릭 처리 로직
    }

    private void OnRightClick(InputAction.CallbackContext _ctx)
    {
        Debug.Log("Player: 오른쪽 클릭 처리");
        // 우클릭 관련 게임 로직
    }

    private void OnUIClick(InputAction.CallbackContext _ctx)
    {
        Debug.Log("UI 클릭 처리");
        // UI 버튼/슬라이더/스크롤 등 처리
    }

    // ------------------------
    // Action Map 전환
    // ------------------------

    public void SwitchToUIInput()
    {
        if (currentActionMap != "UI")
        {
            EnableActionMap("UI");
            Debug.Log("InputActionMap: UI로 전환됨");
        }
    }

    public void SwitchToPlayerInput()
    {
        if (currentActionMap != "Player")
        {
            EnableActionMap("Player");
            Debug.Log("InputActionMap: Player로 복귀됨");
        }
    }

    private void EnableActionMap(string _mapName)
    {
        DisableAllMaps();

        switch (_mapName)
        {
            case "Player":
                actions.Player.Enable();
                break;
            case "UI":
                actions.UI.Enable();
                break;
        }

        currentActionMap = _mapName;
    }

    private void DisableAllMaps()
    {
        actions.Player.Disable();
        actions.UI.Disable();
    }
}
