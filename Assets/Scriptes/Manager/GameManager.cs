using System.Collections;
using UnityEditor.Overlays;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private int selectStageIdx;
    [SerializeField]
    private int selectSubStageIdx;

    [SerializeField]
    private int life;

    private IEnumerator GameFlowRoutine = null;

    [SerializeField]
    private Enemy testEnemy;
    public Vector3 startPos;
    public Vector3 endPos;

    [SerializeField]
    private StageData stageData;

    public int selectStage = 0;
    public bool isDataLoad = false;
    public bool isCreateMap = false;
    public bool isShowUI = false;
    public bool isStartEnemy = false;

    public UIData uiData;

    private Camera mainCamera;
    public Camera MainCamera
    {
        get
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            return mainCamera;
        }
    }
    private void StartGame(StageData _stageData)
    {
        stageData = _stageData;
        MapManager.Instance.SetMap(_stageData);
    }
    private void Update()
    {
        if(isDataLoad)
        {
            DataManager.Instance.LoadGameData();
            isDataLoad = false;
        }

        if (isCreateMap)
        {
            MapManager.Instance.SetMap(DataManager.Instance.GetStageData(selectStage));
            isCreateMap = false;
        }

        if (isShowUI) {
            UIManager.Instance.ShowUI<BaseUI>(uiData);
            isShowUI = false;
        }

        if (isStartEnemy) 
        {
            Enemy enemy = Instantiate(testEnemy, startPos, Quaternion.identity);
            enemy.Spawn(startPos, endPos);
            isStartEnemy = false;
        }
    }
    private void EndGame()
    {

    }
    private IEnumerator CoDataLoadFlow()
    {
        yield return null;
    }
    private IEnumerator CoGameFlow()
    {
        yield return null;
    }
}
