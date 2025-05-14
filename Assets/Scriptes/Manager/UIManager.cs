using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private Transform mainCanvasTrf;
    [SerializeField]
    private Transform contextCanvasTrf;
    [SerializeField]
    private Transform tooltipCanvasTrf;
    [SerializeField]
    private Transform popupCanvasTrf;

    private List<BaseUI> mainUIList = new List<BaseUI>();
    private List<BaseUI> contextUIList = new List<BaseUI>();
    private List<BaseUI> tooltipUIList = new List<BaseUI>();
    private List<BaseUI> popupUIList = new List<BaseUI>();

    public void OpenUI(UIData _data)
    {

    }
    public void HideUI(UIData _data)
    {

    }
}
