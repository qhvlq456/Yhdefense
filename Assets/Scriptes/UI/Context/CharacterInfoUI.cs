using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoUI : BaseUI
{
    [SerializeField]
    private Button upGradeBtn;
    [SerializeField]
    private Button sellBtn;
    [SerializeField]
    private Button exitBtn;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI currentInfoText;
    [SerializeField]
    private TextMeshProUGUI nextInfoText;
    [SerializeField]
    private TextMeshProUGUI totalInfoText;

    [SerializeField]
    private Hero hero;
    private void Awake()
    {
        upGradeBtn.onClick.AddListener(UpGradeBtnClick);
        sellBtn.onClick.AddListener(SellBtnClick);
        exitBtn.onClick.AddListener(ExitBtnClick);
    }
    public void Open(Hero _hero)
    {
        hero = _hero;
        nameText.text = hero.HeroData.name;
        currentInfoText.text = Utility.GetHeroInfo(hero.HeroData, hero.Lv);
        nextInfoText.text = Utility.GetHeroInfo(hero.HeroData, hero.Lv + 1);

        if(hero.Lv + 1 > Utility.HERO_MAX_LV)
        {
            upGradeBtn.interactable = false;
        }
        else
        {
            upGradeBtn.interactable = true;
        }

         totalInfoText.text = hero.GetHeroInfo();
    }
    private void UpGradeBtnClick()
    {
        hero.UpgradeHero();
        Debug.LogError("Upgrade Clicked!");
        // 후에 연출 ㄱㄱ
        DeactivateUI();
    }
    private void SellBtnClick()
    {
        hero.SellHero();
        // 후에 골드 올라가는 연출 ㄱㄱ
        DeactivateUI();
    }
    private void ExitBtnClick()
    {
        DeactivateUI();
    }
}
