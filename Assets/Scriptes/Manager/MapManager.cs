using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [SerializeField]
    private Transform root;
    [SerializeField]
    private List<NavMeshSurface> surfaceList = new List<NavMeshSurface>();
    [SerializeField]
    private NavMeshSurface surfaces = null;
    private List<GameObject> instanceMapObjectList = new List<GameObject>();
    public void SetMap(StageData _stageData)
    {
        StageData stageData = DataManager.Instance.GetStageData(_stageData.index);

        string log = "";

        List<LandData> list = stageData.landDataList;

        for (int i = 0; i < list.Count; i++)
        {
            // 생성하는 부분이 사라졌네??
            Land land = null;
            LandData landData = list[i];
            switch(landData.landType)
            {
                case LandType.hero:
                    land =  ObjectPoolManager.Instance.Create(PoolingType.heroLand, landData.index).GetComponent<HeroLand>();
                    break;
                case LandType.enemy:
                    land = ObjectPoolManager.Instance.Create(PoolingType.enemyLand, landData.index).GetComponent<EnemyLand>();
                    // 동적 navmesh bake를 하기위해 저장
                    //surfaceList.Add(land.GetNavMeshSurface);
                    break;
            }

            land.Create(landData);
            land.transform.SetParent(root);
            land.transform.localPosition = new Vector3(landData.x, 0, landData.z);
            instanceMapObjectList.Add(land.gameObject);
            log += $"landType = {landData.landType}, x = {landData.x} , z = {landData.z}, \n";
        }
        surfaces.BuildNavMesh();
        //Bake();
        // GameManager.Instance.MainCamera.transform.position = new Vector3()
        // 후에 navmesh굽는 작업 필요
        Debug.LogError(log);
    }
    public void ClearMap()
    {
        foreach(var landData in instanceMapObjectList)
        {
            landData.GetComponent<Land>().Retrieve();
        }

        ClearBake();
    }

    public void Bake()
    {
        for (int i = 0; i < surfaceList.Count; i++)
        {
            Debug.LogError("213123");
            surfaceList[i].BuildNavMesh();
        }
    }
    private void ClearBake()
    {
        foreach (var surface in surfaceList)
        {
            surface.RemoveData(); // 또는 RemoveNavMeshData + null 처리
        }

        surfaceList.Clear();
    }
}
