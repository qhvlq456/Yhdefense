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
            land.transform.localPosition = new Vector3(landData.x, 0, landData.z);

            // Track max bounds
            maxX = Mathf.Max(maxX, landData.x);
            maxZ = Mathf.Max(maxZ, landData.z);

            mapTiles.Add(land);

            debugLog += $"landType = {landData.landType}, x = {landData.x}, z = {landData.z}\n";
        }

        // 모든 오브젝트가 자신의 위치를 완전히 잡을 수 있도록 한 프레임 대기
        yield return new WaitForSeconds(0.3f);
        navMeshSurface.BuildNavMesh();

        yield return new WaitForSeconds(0.2f);
        CenterCamera(maxX, maxZ);

        Debug.LogError(debugLog);
    }
    
    private Land CreateLand(LandData landData)
    {
        Land land = null;
        Vector3 pos = new Vector3(landData.x, 0, landData.z);

        switch (landData.landType)
        {
            case LandType.hero:
                land = ObjectPoolManager.Instance.Create(PoolingType.heroLand, landData.index).GetComponent<HeroLand>();
                break;

            case LandType.enemy:
                land = ObjectPoolManager.Instance.Create(PoolingType.enemyLand, landData.index).GetComponent<EnemyLand>();
                SetColorEnemyMap(land, GetEnemyLandType(pos));
                break;
        }

        land.Create(landData);
        return land;
    }

    private EnemyLandType GetEnemyLandType(Vector3 _pos)
    {
        var stageData = GameManager.Instance.CurrentStageData;

        if (Vector3.Equals(stageData.startPoint, _pos)) return EnemyLandType.Start;
        if (Vector3.Equals(stageData.endPoint, _pos)) return EnemyLandType.End;

        return EnemyLandType.Normal;
    }

    private void CenterCamera(float maxX, float maxZ)
    {
        Vector3 camPos = GameManager.Instance.MainCamera.transform.position;
        camPos.x = maxX / 2;
        camPos.z = -maxZ / 2;
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

    private void SetColorEnemyMap(Land land, EnemyLandType type)
    {
        Color color = type switch
        {
            EnemyLandType.Start => setEnemyStartPointColor,
            EnemyLandType.End => setEnemyEndPointColor,
            _ => setEnemyOriginColor
        };

        land.SetColor(color);
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

    public bool IsPossibleSetHero(Vector2Int pos)
    {
        return GetLandList<HeroLand>().Any(land => land.LandData.x == pos.x && land.LandData.z == pos.y);
    }

    private List<T> GetLandList<T>() where T : Land
    {
        return mapTiles.OfType<T>().ToList();
    }
}
