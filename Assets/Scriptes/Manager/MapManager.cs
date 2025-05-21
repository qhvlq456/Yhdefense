using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [SerializeField]
    private Transform root;

    [SerializeField]
    private NavMeshSurface surfaces = null;

    private List<GameObject> instanceMapObjectList = new List<GameObject>();
    public void SetMap(StageData _stageData)
    {
        StageData stageData = DataManager.Instance.GetStageData(_stageData.index);

        string log = "";

        List<LandData> list = stageData.landDataList;
        float maxX = 0, maxZ = 0;
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
                    break;
            }

            land.Create(landData);
            land.transform.SetParent(root);
            land.transform.localPosition = new Vector3(landData.x, 0, landData.z);
            instanceMapObjectList.Add(land.gameObject);
            maxX = Mathf.Max(maxX, landData.x);
            maxZ = Mathf.Max(maxZ, landData.z);
            log += $"landType = {landData.landType}, x = {landData.x} , z = {landData.z}, \n";
        }

        surfaces.BuildNavMesh();
        Vector3 cameraPos = GameManager.Instance.MainCamera.transform.position;
        cameraPos.x = maxX / 2;
        cameraPos.z = maxZ / 2 * -1;
        GameManager.Instance.MainCamera.transform.position = cameraPos;
        // 후에 navmesh굽는 작업 필요
        Debug.LogError(log);
    }
    public void ClearMap()
    {
        foreach(var landData in instanceMapObjectList)
        {
            landData.GetComponent<Land>().Retrieve();
        }

        surfaces.RemoveData();
    }
}
