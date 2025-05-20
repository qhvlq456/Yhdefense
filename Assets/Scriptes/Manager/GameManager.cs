using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private float enemySpawnDelay = 0.3f;

    private IEnumerator GameFlowRoutine = null;
    private IEnumerator EnemySpawnRoutine = null;

    [SerializeField]
    private StageData currentStageData;
    [SerializeField]
    private int currentSubStageIdx;
    [SerializeField]
    public int life { private set; get; }
    // 후에 private set
    [SerializeField]
    public int gold { set; get; }

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
    public void StartGame(StageData _stageData)
    {
        currentStageData = _stageData;
        // 후에 변경하기
        currentSubStageIdx = 0;
        life = currentStageData.life;

        MapManager.Instance.SetMap(_stageData);
        SubStageData subStageData = DataManager.Instance.GetIdxToSubStageData(_stageData.subStageIdxList[currentSubStageIdx]);
        string log = "";
        for(int i = 0; i < _stageData.subStageIdxList.Count; i++)
        {
            log += $"{i} : {_stageData.subStageIdxList[i]}, ";
        }
        Debug.LogError($"{log}");
        StartEnemySpawn(subStageData);
    }
    public void EndGame()
    {
        MapManager.Instance.ClearMap();
        CharacterManager.Instance.ClearCharacter();
        UIManager.Instance.HideAllUI();
        UIManager.Instance.RecycleAllUI();
    }
    private IEnumerator CoDataLoadFlow()
    {
        yield return null;
    }
    private IEnumerator CoGameFlow()
    {
        yield return null;
    }

    public void StartEnemySpawn(SubStageData _subStageData)
    {
        if(EnemySpawnRoutine != null)
        {
            StopCoroutine(EnemySpawnRoutine);
        }

        EnemySpawnRoutine = CoEnemySpawnFlow(_subStageData);
        StartCoroutine(EnemySpawnRoutine);
    }
    private IEnumerator CoEnemySpawnFlow(SubStageData _subStageData)
    {
        Vector3 startPoint = currentStageData.startPoint;
        Vector3 endPoint = currentStageData.endPoint;

        List<int> enemyList = _subStageData.enemyIdxList;

        Debug.LogError($"[GameManager] [CoEnemySpawnFlow] startPoint : {startPoint}, endPoint : {endPoint} enemyList cnt : {enemyList.Count}");
        for (int i = 0; i < enemyList.Count; i++) 
        {
            int idx = enemyList[i];
            CharacterManager.Instance.SpawnEnemy(idx, startPoint, endPoint);
            yield return new WaitForSeconds(enemySpawnDelay);
        }

        yield return null;
    }
}
