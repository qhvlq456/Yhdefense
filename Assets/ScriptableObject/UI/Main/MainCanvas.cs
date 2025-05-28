using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : BaseCanvas
{
    OldPlayerController oldPlayerController = null;
    [SerializeField]
    private Button createBtn;

    [SerializeField]
    private Button cancelHeroBtn;
    private void Awake()
    {
        oldPlayerController = FindAnyObjectByType<OldPlayerController>();
        cancelHeroBtn.gameObject.SetActive(false);
    }
    public void CreateHeroBtnClick()
    {
        CreateHeroUI createHeroUI = UIManager.Instance.ShowUI<CreateHeroUI>(UIPanelType.CreateHero);
        createHeroUI.Open(DataManager.Instance.HeroDataList);
        cancelHeroBtn.gameObject.SetActive(true);
    }
    public void CancelHeroBtnClick()
    {
        oldPlayerController.ExitPlacementMode();
        cancelHeroBtn.gameObject.SetActive(false);
    }

    public void ExitBtnClick()
    {

    }
}
