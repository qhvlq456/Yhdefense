using UnityEngine;

public class TestScene : MonoBehaviour
{
    private Vector2 position = new Vector2(10, 10);
    private Vector2 btnSize = new Vector2(200, 50);
    private Vector2 textSize = new Vector2(200, 50);
    GUIStyle btnGUIStyle;
    GUIStyle textFieldStyle;
    
    // stage
    private string stageIdxStr = string.Empty;
    private string subStageIdxStr = string.Empty;
    public StageData currentStageData;
    private void OnGUI()
    {
        btnGUIStyle = new GUIStyle(GUI.skin.button);
        textFieldStyle = new GUIStyle(GUI.skin.textField);

        btnGUIStyle.fontSize = 20;
        textFieldStyle.alignment = TextAnchor.MiddleCenter;

        if (GUI.Button(new Rect(position.x, 10, btnSize.x, btnSize.y), "������ �ε�", btnGUIStyle))
        {
            DataManager.Instance.LoadGameData();
        }

        stageIdxStr = GUI.TextField(new Rect(position.x + textSize.x + 30, 70, textSize.x, textSize.y), stageIdxStr, textFieldStyle);
        if (GUI.Button(new Rect(position.x, 70, btnSize.x, btnSize.y), "�� �ε�", btnGUIStyle))
        {
            Debug.LogError("�� �ε�");
            currentStageData = DataManager.Instance.GetStageData(int.Parse(stageIdxStr));
            MapManager.Instance.SetMap(currentStageData);
        }

        if (GUI.Button(new Rect(position.x, 140, btnSize.x, btnSize.y), "�� ȸ��", btnGUIStyle))
        {
            MapManager.Instance.ClearMap();
        }

        subStageIdxStr = GUI.TextField(new Rect(position.x + textSize.x + 30, 210, textSize.x, textSize.y), subStageIdxStr, textFieldStyle);
        if (GUI.Button(new Rect(position.x, 210, btnSize.x, btnSize.y), "�� ����", btnGUIStyle))
        {
            currentStageData = DataManager.Instance.GetStageData(int.Parse(stageIdxStr));
            MapManager.Instance.ClearMap();
            GameManager.Instance.StartGame(currentStageData);
        }

        if (GUI.Button(new Rect(position.x, 280, btnSize.x, btnSize.y), "��� ȸ��", btnGUIStyle))
        {
            GameManager.Instance.EndGame();
        }

        //if (isShowUI)
        //{
        //    UIManager.Instance.ShowUI<BaseUI>(uiData);
        //    isShowUI = false;
        //}
    }
}
