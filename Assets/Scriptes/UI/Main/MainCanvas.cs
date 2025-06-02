using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : BaseCanvas
{
    OldPlayerController oldPlayerController = null;
    [Header("Gold")]
    [SerializeField]
    private TextMeshProUGUI goldText;
    [Header("Life")]
    [SerializeField]
    private TextMeshProUGUI lifeText;
    [Header("Stage")]
    [SerializeField]
    private TextMeshProUGUI stageText;

    [Header("Create Hero")]
    [SerializeField]
    private Button createBtn;

    [Header("Cancel Hero")]
    [SerializeField]
    private Button cancelHeroBtn;
    private void Awake()
    {
        oldPlayerController = FindAnyObjectByType<OldPlayerController>();
        cancelHeroBtn.gameObject.SetActive(false);
    }
    public override void UpdateCanvas()
    {
        UpdateGoldText();
        UpdateLifeText(); 
        UpdateStageText();
    }
    public void UpdateGoldText()
    {
        goldText.text = string.Format("°ñµå : {0}", GameManager.Instance.gold);
    }
    public void UpdateLifeText()
    {

    }
    public void UpdateStageText()
    {

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
