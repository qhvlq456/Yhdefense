using UnityEngine;
using UnityEngine.UI;

public class AlertPopupUI : BaseUI
{
    [SerializeField]
    private Button exitBtn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // юс╫ц©К
        exitBtn.onClick.AddListener(HideUI);
    }


}
