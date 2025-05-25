using System.Data.Common;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CreateHeroInfoItemUI : MonoBehaviour
{
    [SerializeField]
    private HeroData heroData;
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private Image heroImg;
    [SerializeField]
    private TextMeshProUGUI infoText;
    [SerializeField]
    private Button selectBtn;

    private System.Action callback = null;

    public void Set(HeroData _heroData, System.Action _callback)
    {
        selectBtn.onClick.RemoveAllListeners();
        selectBtn.onClick.AddListener(SelectBtnClick);
        callback = _callback;
        titleText.text = _heroData.name;
        infoText.text = Utility.GetHeroInfo(_heroData);
    }

    public void SelectBtnClick()
    {
        if(Utility.IsHeroPurchase(heroData))
        {
            // 임시로 설정 후에 변경 예정
            OldPlayerController oldPlayerController = FindAnyObjectByType<OldPlayerController>();
            oldPlayerController.EnterPlacementMode(heroData);
            callback?.Invoke();
        }
        else
        {
            UIManager.Instance.ShowUI<AlertPopupUI>(UIPanelType.AlertPopup);
        }
    }

}
