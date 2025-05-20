using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LobyPanelUI : BaseUI
{
    [SerializeField]
    private GameObject btnItemRes;

    [SerializeField]
    private List<LobyPanelItemUI> itemBtnList = new List<LobyPanelItemUI>();
    [SerializeField]
    private ScrollRect scrollRect;

    [SerializeField]
    private Button exitBtn;
    public override void ShowUI()
    {
        // exitBtn.clickable.clicked += () => HideUI();
        exitBtn.onClick.AddListener(HideUI);
    }


}
