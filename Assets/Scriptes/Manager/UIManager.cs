using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] 
    private UIDataDB uiDataDB;

    [SerializeField] 
    private List<CanvasMap> canvasMaps;

    private Dictionary<UIType, Canvas> canvasDic;                   // UIType �� Canvas ����
    private Dictionary<UIPanelType, BaseUI> uiInstances;            // ���� UI �ν��Ͻ�
    private Dictionary<UIPanelType, Queue<BaseUI>> uiPoolDic;       // ���� ������ UI Ǯ

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
    /// ���� UI ǥ�� (�̹� ������ UI�� ������ ��Ȱ��)
    /// </summary>
    public T ShowUI<T>(UIPanelType panelType) where T : BaseUI
    {
        if (uiInstances.TryGetValue(panelType, out var ui))
        {
            ui.ShowUI();
            ui.transform.SetAsLastSibling(); // �ֻ������
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
    /// ���� UI�� ���� �� �����ϰ� ���� �� (ex. HPBar)
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

        // Ǯ���� ����� ���
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
    /// ���� UI�� ȸ���� �� ����ϴ� �Լ� (���� ȣ�� �ʿ�)
    /// </summary>
    public void RecycleUI(UIPanelType panelType, BaseUI ui)
    {
        var res = uiDataDB.Get(panelType);

        if (res == null || !res.usePooling)
        {
            Destroy(ui.gameObject); // Ǯ�� �� �ϸ� �׳� �ı�
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
    /// ��� ���� UI�� ȸ�� (���� UI ����)
    /// </summary>
    public void RecycleAllUI()
    {
        foreach (var kvp in uiPoolDic)
        {
            var panelType = kvp.Key;
            var canvas = canvasDic[uiDataDB.Get(panelType).canvasType];

            // ���� Ȱ��ȭ�� ������Ʈ �� ��Ȱ�� �� �� �� ã�Ƽ� ��Ȱ��ȭ
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
