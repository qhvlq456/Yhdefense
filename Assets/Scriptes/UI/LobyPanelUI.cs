using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LobyPanelUI : BaseUI
{
    [SerializeField]
    private GameObject btnItemRes;

    [SerializeField]
    private List<LobyPanelUIItemBtn> itemBtnList = new List<LobyPanelUIItemBtn>();
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
