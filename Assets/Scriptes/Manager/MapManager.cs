using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private Transform root;
    [SerializeField] private NavMeshSurface navMeshSurface;

    [Header("Map Color")]
    [SerializeField] private Color setHeroPossibleColor = Color.green;
    [SerializeField] private Color setHeroImpossibleColor = Color.red;
    [SerializeField] private Color setHeroOriginColor = Color.white;

    [SerializeField] private Color setEnemyOriginColor = Color.black;
    [SerializeField] private Color setEnemyStartPointColor = Color.blue;
    [SerializeField] private Color setEnemyEndPointColor = Color.yellow;

    private List<Land> mapTiles = new();

    private enum EnemyLandType { Normal, Start, End }

    public IEnumerator SetMapAsync(StageData _stageData)
    {
        ClearMap();

        float maxX = 0f, maxZ = 0f;
        string debugLog = string.Empty;

        foreach (var landData in _stageData.landDataList)
        {
            Land land = CreateLand(landData);

            // Set parent and position
            land.transform.SetParent(root);
            land.transform.localPosition = new Vector3(landData.x * DataManager.Instance.X_OFFSET, 0, landData.z * DataManager.Instance.Z_OFFSET);

            // Track max bounds
            maxX = Mathf.Max(maxX, landData.x);
            maxZ = Mathf.Max(maxZ, landData.z);

            mapTiles.Add(land);

            debugLog += $"landType = {landData.landType}, x = {landData.x}, z = {landData.z}\n";
        }
        yield return null;
        Debug.LogError($"Start MapManager.SetMapAsync()");
        // 모든 오브젝트가 자신의 위치를 완전히 잡을 수 있도록 한 프레임 대기
        navMeshSurface.BuildNavMesh();
        Debug.LogError($"Finish MapManager.SetMapAsync()");
        CenterCamera(maxX, maxZ);

        Debug.LogError(debugLog);
    }
    
    private Land CreateLand(LandData _landData)
    {
        Land land = null;
        Vector3 pos = new Vector3(_landData.x, 0, _landData.z);

        switch (_landData.landType)
        {
            case LandType.hero:
                land = ObjectPoolManager.Instance.Create(PoolingType.heroLand, _landData.index).GetComponent<HeroLand>();
                break;

            case LandType.enemy:
                land = ObjectPoolManager.Instance.Create(PoolingType.enemyLand, _landData.index).GetComponent<EnemyLand>();
                SetColorEnemyMap(land, GetEnemyLandType(pos));
                break;
        }

        land.Create(_landData);
        return land;
    }

    private EnemyLandType GetEnemyLandType(Vector3 _pos)
    {
        var stageData = GameManager.Instance.CurrentStageData;

        if (Vector3.Equals(stageData.startPoint, _pos)) return EnemyLandType.Start;
        if (Vector3.Equals(stageData.endPoint, _pos)) return EnemyLandType.End;

        return EnemyLandType.Normal;
    }

    private void CenterCamera(float _maxX, float _maxZ)
    {
        Vector3 camPos = GameManager.Instance.MainCamera.transform.position;
        camPos.x = _maxX / 2 * DataManager.Instance.X_OFFSET;
        camPos.z = -_maxZ / 2 * DataManager.Instance.Z_OFFSET;

        GameManager.Instance.MainCamera.transform.position = camPos;
    }

    public void ClearMap()
    {
        foreach (var land in mapTiles)
        {
            land.Retrieve();
        }

        mapTiles.Clear();
        navMeshSurface.RemoveData();
    }

    private void SetColorEnemyMap(Land _land, EnemyLandType _type)
    {
        Color color = _type switch
        {
            EnemyLandType.Start => setEnemyStartPointColor,
            EnemyLandType.End => setEnemyEndPointColor,
            _ => setEnemyOriginColor
        };

        _land.SetColor(color);
    }

    public void SetColorHeroMap()
    {
        foreach (var land in GetLandList<HeroLand>())
        {
            land.SetColor(land.IsHeroEmpty ? setHeroPossibleColor : setHeroImpossibleColor);
        }
    }

    public void SetHeroOriginalColor()
    {
        foreach (var land in GetLandList<HeroLand>())
        {
            land.SetColor(setHeroOriginColor);
        }
    }

    public bool IsPossibleSetHero(Vector2Int _pos)
    {
        return GetLandList<HeroLand>().Any(land => land.LandData.x * DataManager.Instance.X_OFFSET == _pos.x && land.LandData.z * DataManager.Instance.Z_OFFSET == _pos.y);
    }

    private List<T> GetLandList<T>() where T : Land
    {
        return mapTiles.OfType<T>().ToList();
    }
}
