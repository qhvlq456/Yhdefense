using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

public class UIManager : Singleton<UIManager>
{
    [Serializable]
    public class UIControllerMap
    {
        public UIType type;
        public Canvas parent;

        private readonly List<BaseUI> uiList = new();

        public T EnableUI<T>(UIData _uiData) where T : BaseUI
        {
            T ret = FindUI(_uiData) as T;

            if (ret != null)
            {
                if (!ret.gameObject.activeSelf)
                {
                    ret.gameObject.SetActive(true);
                    ret.ShowUI(_uiData);
                    ret.transform.SetAsLastSibling();
                }
            }
            else
            {
                ret = DataManager.Instance.GetUIResObj(_uiData).GetComponent<T>();

                if (ret == null)
                {
                    Debug.LogError($"[UIControllerMap] Failed to load UI: {_uiData.panelType}");
                    return null;
                }

                uiList.Add(ret);
                ret.transform.SetParent(parent.transform, false);
                ret.ShowUI(_uiData);
                ret.gameObject.SetActive(true);
                ret.transform.SetAsLastSibling();
            }

            return ret;
        }

        public void DisableUI(UIData _uiData)
        {
            BaseUI baseUI = FindUI(_uiData);

            if (baseUI != null)
            {
                baseUI.HideUI();
                baseUI.gameObject.SetActive(false);
                baseUI.transform.SetAsFirstSibling();
                uiList.Remove(baseUI);
            }
            else
            {
                Debug.LogWarning($"[UIControllerMap] UI to disable not found: {_uiData.panelType}");
            }
        }

        private BaseUI FindUI(UIData _uiData)
        {
            return uiList.Find(ui => ui.GetUIData().panelType == _uiData.panelType);
        }

        public bool HasAnyActiveUI()
        {
            return uiList.Exists(ui => ui.gameObject.activeSelf);
        }
    }


    [SerializeField]
    private PlayerController playerController;
    private List<UIControllerMap> uiControllerMapList = new List<UIControllerMap>();

    private UIControllerMap GetTypeToUIController(UIType _uiType) => uiControllerMapList.Find(x => x.type == _uiType);

    public T ShowUI<T>(UIData _data) where T : BaseUI
    {
        var controller = GetTypeToUIController(_data.type);

        if (controller == null)
        {
            Debug.LogError($"[UIManager] ShowUI: No UIController for type {_data.type}");
            return null;
        }

        // UI 열릴 때만 Input 전환
        if (!controller.HasAnyActiveUI())
        {
            playerController.SwitchToUIInput();
        }

        return controller.EnableUI<T>(_data);
    }

    public void HideUI(UIData _data)
    {
        var controller = GetTypeToUIController(_data.type);

        if (controller == null)
        {
            Debug.LogError($"[UIManager] HideUI: No UIController for type {_data.type}");
            return;
        }

        controller.DisableUI(_data);

        // 모든 UI가 꺼졌는지 확인
        bool allClosed = uiControllerMapList.TrueForAll(x => !x.HasAnyActiveUI());

        if (allClosed)
        {
            playerController.SwitchToPlayerInput();
        }
    }

    private void CalculateUI()
    {

    }
    public void AllHideUI()
    {

    }
    public void AllDestoryUI()
    {

    }
}
