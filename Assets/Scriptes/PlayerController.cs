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

        // Player ActionMap Ŭ�� ���ε�
        actions.Player.LeftClick.performed += OnLeftClick;
        actions.Player.RightClick.performed += OnRightClick;

        // UI ActionMap Ŭ�� ���ε�
        actions.UI.Click.performed += OnUIClick;
    }

    private void OnEnable()
    {
        actions.Player.Enable(); // �⺻������ Player �� Ȱ��ȭ
    }

    private void OnDisable()
    {
        actions.Player.Disable();
        actions.UI.Disable();
    }

    private void OnLeftClick(InputAction.CallbackContext _ctx)
    {
        Debug.Log("���� Ŭ��");
    }

    private void OnRightClick(InputAction.CallbackContext _ctx)
    {
        Debug.Log("������ Ŭ��");
    }

    private void OnUIClick(InputAction.CallbackContext _ctx)
    {
        Debug.Log("UI Ŭ��");
    }


    /// <summary>
    /// UI�� ���� �� ȣ�� (�ܺο���)
    /// </summary>
    public void SwitchToUIInput()
    {
        if (actions.Player.enabled)
        {
            actions.Player.Disable();
            previousActionMap = "Player";
        }

        actions.UI.Enable();
        Debug.Log("Input ActionMap: UI�� ��ȯ��");
    }

    /// <summary>
    /// UI�� ���� �� ȣ�� (�ܺο���)
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
            Debug.Log("Input ActionMap: Player�� ���͵�");
        }
    }
}
