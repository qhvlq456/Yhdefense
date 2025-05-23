using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class TestScene : MonoBehaviour
{
    [SerializeField]
    private GameObject TestEnemy;
    
    private Vector2 position = new Vector2(10, 10);
    private Vector2 btnSize = new Vector2(200, 50);
    private Vector2 textSize = new Vector2(200, 50);
    GUIStyle btnGUIStyle;
    GUIStyle textFieldStyle;
    
    // stage
    private string stageIdxStr = string.Empty;
    private string subStageIdxStr = string.Empty;
    private string addGoldStr = string.Empty;
    public StageData currentStageData;
    private void OnGUI()
    {
        btnGUIStyle = new GUIStyle(GUI.skin.button);
        textFieldStyle = new GUIStyle(GUI.skin.textField);

        btnGUIStyle.fontSize = 20;
        textFieldStyle.alignment = TextAnchor.MiddleCenter;

        if (GUI.Button(new Rect(position.x, 10, btnSize.x, btnSize.y), "데이터 로드", btnGUIStyle))
        {
            DataManager.Instance.LoadGameData();
        }

        stageIdxStr = GUI.TextField(new Rect(position.x + textSize.x + 30, 70, textSize.x, textSize.y), stageIdxStr, textFieldStyle);
        if (GUI.Button(new Rect(position.x, 70, btnSize.x, btnSize.y), "멥 로드", btnGUIStyle))
        {
            Debug.LogError("멥 로드");
            currentStageData = DataManager.Instance.GetStageData(int.Parse(stageIdxStr));
            MapManager.Instance.SetMap(currentStageData);
        }

        if (GUI.Button(new Rect(position.x, 140, btnSize.x, btnSize.y), "멥 회수", btnGUIStyle))
        {
            MapManager.Instance.ClearMap();
        }

        subStageIdxStr = GUI.TextField(new Rect(position.x + textSize.x + 30, 210, textSize.x, textSize.y), subStageIdxStr, textFieldStyle);
        if (GUI.Button(new Rect(position.x, 210, btnSize.x, btnSize.y), "적 스폰", btnGUIStyle))
        {
            currentStageData = DataManager.Instance.GetStageData(int.Parse(stageIdxStr));
            MapManager.Instance.ClearMap();
            GameManager.Instance.StartGame(currentStageData);
        }

        if (GUI.Button(new Rect(position.x, 280, btnSize.x, btnSize.y), "모두 회수", btnGUIStyle))
        {
            GameManager.Instance.EndGame();
        }
        
        if (GUI.Button(new Rect(position.x, 350, btnSize.x, btnSize.y), "테스트 적 소환", btnGUIStyle))
        {
            CharacterManager.Instance.SpawnEnemy(0, currentStageData.startPoint, currentStageData.endPoint);
        }

        addGoldStr = GUI.TextField(new Rect(position.x + textSize.x + 30, 420, textSize.x, textSize.y), addGoldStr, textFieldStyle);
        if (GUI.Button(new Rect(position.x, 420, btnSize.x, btnSize.y), "골드 추가", btnGUIStyle))
        {
            GameManager.Instance.gold = int.Parse(addGoldStr);
        }
        //if (isShowUI)
        //{
        //    UIManager.Instance.ShowUI<BaseUI>(uiData);
        //    isShowUI = false;
        //}
    }
}
