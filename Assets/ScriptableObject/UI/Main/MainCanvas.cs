using UnityEngine;

public class MainCanvas : BaseCanvas
{
    public void CreateHeroBtnClick()
    {
        CreateHeroUI createHeroUI = UIManager.Instance.ShowUI<CreateHeroUI>(UIPanelType.CreateHero);
        createHeroUI.Open(DataManager.Instance.HeroDataList);
    }

    public void ExitBtnClick()
    {

    }
}
