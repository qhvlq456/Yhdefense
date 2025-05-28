using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : BaseCanvas
{
    OldPlayerController oldPlayerController = null;
    [SerializeField]
    private Button createOrCacnelBtn;
    private void Awake()
    {
        oldPlayerController = FindAnyObjectByType<OldPlayerController>();
    }
    public void CreateHeroOrCancelBtnClick()
    {
        Debug.LogError($"oldPlayerController.CurrentState : {oldPlayerController.CurrentState}");
        switch (oldPlayerController.CurrentState)
        {
            case OldPlayerController.InputState.none:
                CreateHeroUI createHeroUI = UIManager.Instance.ShowUI<CreateHeroUI>(UIPanelType.CreateHero);
                createHeroUI.Open(DataManager.Instance.HeroDataList);
                createOrCacnelBtn.GetComponentInChildren<TextMeshProUGUI>().text = "취소";
                break;
            case OldPlayerController.InputState.placement:
                oldPlayerController.ExitPlacementMode();
                createOrCacnelBtn.GetComponentInChildren<TextMeshProUGUI>().text = "영웅 생성";
                break;
            default: 
                Debug.LogError("Unknown InputState: " + oldPlayerController.CurrentState);
                break;
        }        
    }

    public void ExitBtnClick()
    {

    }
}
