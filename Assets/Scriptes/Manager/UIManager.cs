using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] 
    private UIDataDB uiDataDB;

    [SerializeField] 
    private List<CanvasMap> canvasMaps;

    private Dictionary<UIType, Canvas> canvasDic;                   // UIType 별 Canvas 참조
    private Dictionary<UIPanelType, BaseUI> uiInstances;            // 단일 UI 인스턴스
    private Dictionary<UIPanelType, Queue<BaseUI>> uiPoolDic;       // 재사용 가능한 UI 풀

    [Serializable]
    private class CanvasMap
    {
        public UIType uiType;
        public Canvas canvas;
    }

    protected override void Awake()
    {
        base.Awake();
        canvasDic = new();
        uiInstances = new();
        uiPoolDic = new();

        foreach (var map in canvasMaps)
            canvasDic[map.uiType] = map.canvas;

        uiDataDB.Initialize();
    }

    /// <summary>
    /// 단일 UI 표시 (이미 생성된 UI가 있으면 재활용)
    /// </summary>
    public T ShowUI<T>(UIPanelType panelType) where T : BaseUI
    {
        if (uiInstances.TryGetValue(panelType, out var ui))
        {
            ui.ShowUI();
            ui.transform.SetAsLastSibling(); // 최상단으로
            return ui as T;
        }

        var res = uiDataDB.Get(panelType);
        if (res == null || !canvasDic.TryGetValue(res.canvasType, out var canvas))
        {
            Debug.LogError($"[UIManager] UIData or Canvas missing: {panelType}");
            return null;
        }

        var instance = Instantiate(res.prefab, canvas.transform).GetComponent<T>();
        instance.Initialize(panelType);
        instance.ShowUI();
        instance.transform.SetAsLastSibling();
        uiInstances[panelType] = instance;

        return instance;
    }

    /// <summary>
    /// 동일 UI를 여러 개 생성하고 싶을 때 (ex. HPBar)
    /// </summary>
    public T ShowMultipleUI<T>(UIPanelType panelType) where T : BaseUI
    {
        var res = uiDataDB.Get(panelType);
        if (res == null || !canvasDic.TryGetValue(res.canvasType, out var canvas))
        {
            Debug.LogError($"[UIManager] UIData or Canvas missing: {panelType}");
            return null;
        }

        T ret = null;

        // 풀링을 사용할 경우
        if (res.usePooling)
        {
            if (!uiPoolDic.TryGetValue(panelType, out var pool))
                uiPoolDic[panelType] = pool = new Queue<BaseUI>();

            if (pool.Count > 0)
            {
                ret = pool.Dequeue().GetComponent<T>();
                ret.gameObject.SetActive(true);
            }
        }

        if (ret == null)
            ret = Instantiate(res.prefab, canvas.transform).GetComponent<T>();

        ret.Initialize(panelType);
        ret.ShowUI();
        ret.transform.SetAsLastSibling();

        return ret;
    }

    /// <summary>
    /// 다중 UI를 회수할 때 사용하는 함수 (직접 호출 필요)
    /// </summary>
    public void RecycleUI(UIPanelType panelType, BaseUI ui)
    {
        var res = uiDataDB.Get(panelType);

        if (res == null || !res.usePooling)
        {
            Destroy(ui.gameObject); // 풀링 안 하면 그냥 파괴
            return;
        }

        if (!uiPoolDic.TryGetValue(panelType, out var pool))
            pool = uiPoolDic[panelType] = new Queue<BaseUI>();

        ui.HideUI();
        ui.gameObject.SetActive(false);
        pool.Enqueue(ui);
    }

    public void HideUI(UIPanelType panelType)
    {
        if (uiInstances.TryGetValue(panelType, out var ui))
            ui.HideUI();
    }

    public void HideAllUI()
    {
        foreach (var ui in uiInstances.Values)
            ui.HideUI();
    }
    /// <summary>
    /// 모든 재사용 UI를 회수 (다중 UI 전용)
    /// </summary>
    public void RecycleAllUI()
    {
        foreach (var kvp in uiPoolDic)
        {
            var panelType = kvp.Key;
            var canvas = canvasDic[uiDataDB.Get(panelType).canvasType];

            // 현재 활성화된 오브젝트 중 재활용 안 된 것 찾아서 비활성화
            foreach (Transform child in canvas.transform)
            {
                var ui = child.GetComponent<BaseUI>();
                if (ui != null && ui.panelType == panelType && ui.gameObject.activeSelf)
                {
                    RecycleUI(panelType, ui);
                }
            }
        }
    }

    public void DestroyUI(UIPanelType panelType)
    {
        if (uiInstances.TryGetValue(panelType, out var ui))
        {
            Destroy(ui.gameObject);
            uiInstances.Remove(panelType);
        }
    }

    public void DestroyAllUI()
    {
        foreach (var ui in uiInstances.Values)
            Destroy(ui.gameObject);

        uiInstances.Clear();
    }
}
