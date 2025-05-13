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
    private StageData stageData;

    public int selectStage = 0;
    public bool isDataLoad = false;
    public bool isCreateMap = false;

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
