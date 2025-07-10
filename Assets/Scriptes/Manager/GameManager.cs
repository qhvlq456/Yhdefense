using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private float enemySpawnDelay = 0.3f;

    private IEnumerator GameFlowRoutine = null;
    private IEnumerator EnemySpawnRoutine = null;

    [SerializeField]
    private StageData currentStageData;
    public StageData CurrentStageData => currentStageData;
    [SerializeField]
    private int currentSubStageIdx;
    [SerializeField]
    public int life { private set; get; }
    // 후에 private set
    [SerializeField]
    public int gold;
    public void UpdateGold(int _gold)
    { 
        gold = Mathf.Clamp(gold + _gold, 0, int.MaxValue);
        UIManager.Instance.UpdateCanvas(UIType.main);
    }

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
    protected override void Awake()
    {
        base.Awake();        
    }
    public void StartGame(StageData _stageData)
    {
        gold = 0;
        currentStageData = _stageData;
        // 후에 변경하기
        currentSubStageIdx = 0;
        life = currentStageData.life;

        string log = "";
        for(int i = 0; i < _stageData.subStageIdxList.Count; i++)
        {
            log += $"{i} : {_stageData.subStageIdxList[i]}, ";
        }
        Debug.LogError($"{log}");
        StartCoroutine(CoGameFlow());
    }
    public void EndGame()
    {
        MapManager.Instance.ClearMap();
        CharacterManager.Instance.AllClearCharacter();
        UIManager.Instance.HideAllUI();
        UIManager.Instance.RecycleAllUI();
    }
    private IEnumerator CoDataLoadFlow()
    {
        yield return null;
    }
    private IEnumerator CoGameFlow()
    {
        // 1. NavMesh 빌드 완료까지 기다리는 흐름
        yield return StartCoroutine(MapManager.Instance.SetMapAsync(currentStageData));

        // 2. SubStageData 가져오기
        SubStageData subStageData = DataManager.Instance.GetIdxToSubStageData(currentStageData.subStageIdxList[currentSubStageIdx]);

        // 3. 로그
        string log = string.Join(", ", currentStageData.subStageIdxList.Select((val, idx) => $"{idx}: {val}"));
        Debug.LogError($"[GameManager] SubStageIdxList: {log}");
        // 4. 적 스폰 시작
        StartEnemySpawn(subStageData);
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
