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

    private System.Action<int> onCreateHero;

    public void Set(HeroData _heroData, System.Action<int> _onCreateHero)
    {
        selectBtn.onClick.RemoveAllListeners();
        selectBtn.onClick.AddListener(SelectBtnClick);
        onCreateHero = _onCreateHero;
        titleText.text = _heroData.name;
        infoText.text = Utility.GetHeroInfo(_heroData);
    }

    public void SelectBtnClick()
    {
        if(Utility.IsHeroPurchase(heroData))
        {
            // select 이후 설치 가능한 곳은 녹색으로 번쩍, 불가능한곳 빨간색으로 번쩍, enemy land는 없음
            // 설치 가능한 곳에 마우스를 인풋 업 하면 해당 heroland에 hero가 설치 빨간색이면 그냥 아무것도 없음
            // heroland in hero set
            onCreateHero?.Invoke(heroData.index);
        }
        else
        {
            UIManager.Instance.ShowUI<AlertPopupUI>(UIPanelType.AlertPopup);
        }
    }

}
