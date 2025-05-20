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
            // heroland in hero set
            onCreateHero?.Invoke(heroData.index);
        }
        else
        {
            UIManager.Instance.ShowUI<AlertPopupUI>(UIPanelType.AlertPopup);
        }
    }

}
