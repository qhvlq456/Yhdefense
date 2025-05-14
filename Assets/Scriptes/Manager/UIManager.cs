using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System;

public class UIManager : Singleton<UIManager>
{
    [Serializable]
    public class UIControllerMap
    {
        public UIType type;
        public Canvas parent;
        private List<BaseUI> uiList = new List<BaseUI>();

        public T EnableUI<T>(UIData _uiData) where T : BaseUI
        {
            T ret = FindUI(_uiData) as T;

            if (ret != null)
            {
                ret.ShowUI(_uiData);
            }
            else
            {
                ret = DataManager.Instance.GetUIResObj(_uiData).GetComponent<T>();
                uiList.Add(ret);
            }

            ret.transform.SetParent(parent.transform);
            ret.gameObject.SetActive(true);
            // 마지막으로 옮겨서 뎁스 조정
            ret.transform.SetAsLastSibling();
            return ret;
        }
        private BaseUI FindUI(UIData _uiData)
        {
            BaseUI baseUI = null;

            for (int i = 0; i < uiList.Count; i++)
            {
                if(uiList[i].data.panelType == _uiData.panelType)
                {
                    baseUI = uiList[i];
                }
            }

            return baseUI;
        }
        public void DisableUI(UIData _uiData)
        {
            BaseUI baseUI = FindUI(_uiData);

            if (baseUI != null)
            {
                // 첫번째 자식으로 옮겨서 뎁스 조정
                uiList.Remove(baseUI);
                baseUI.transform.SetAsFirstSibling();
                baseUI.HideUI();
                baseUI.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError($"Not Exist BaseUI!! uiType : {_uiData.type} panelType : {_uiData.panelType}");
            }

        }
    }

    private List<UIControllerMap> uiControllerMapList = new List<UIControllerMap>();

    private UIControllerMap GetTypeToUIController(UIType _uiType) => uiControllerMapList.Find(x => x.type == _uiType);


    public T ShowUI<T>(UIData _data) where T : BaseUI
    {
        T ret = null;

        UIControllerMap uiControllerMap = GetTypeToUIController(_data.type);

        if (uiControllerMap != null)
        {
            ret = uiControllerMap.EnableUI<T>(_data);
        }
        else
        {
            Debug.LogError($"[UIManager] ShowUI Not Exist uidata type : {_data.type}, panelType : {_data.panelType}");
        }

        return ret;
    }
    public void HideUI(UIData _data)
    {
        UIControllerMap uiControllerMap = GetTypeToUIController(_data.type);

        if (uiControllerMap != null)
        {
            uiControllerMap.DisableUI(_data);
        }
        else
        {
            Debug.LogError($"[UIManager] HideUI Not Exist uidata type : {_data.type}, panelType : {_data.panelType}");
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
