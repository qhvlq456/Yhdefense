using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>, ILoader
{
    [SerializeField]
    public GameObject landResObj { get; private set; }

    [SerializeField]
    private Transform root;

    public const int MAX_X = 20;
    public const int MAX_Z = 20;
    public int TotalLandNum => MAX_X * MAX_Z;

    [SerializeField]
    private int xOffset = 1;
    [SerializeField]
    private int zOffset = 1;

    // Data
    [SerializeField]
    private List<MapData> mapDataList = new List<MapData>();

    private List<GameObject> instanceMapObjectList = new List<GameObject>();

    public int Priority { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    
    public void Load(string _path)
    {
        List<StageData> list = NewtonSoftJson.LoadJsonArray<StageData>(Application.streamingAssetsPath, "MapData");

        for (int i = 0; i < list.Count; i++)
        {
            MapData data = new MapData();
            data.index = i;
            data.stageData = list[i];
        }
    }
    /// <summary>
    /// ������ 0,0 �������� ����� �̵� �� ī�޶� �߾ӿ� �����
    /// </summary>
    /// <param name="_stage">������ ������ ��������</param>
    public void SetMap(int _stage, int _subStage)
    {
        StageData stageData = mapDataList[_stage].stageData;

        string log = "";

        List<LandData> list = stageData.landDataList;

        for (int i = 0; i < list.Count; i++)
        {
            // �����ϴ� �κ��� �������??
            Land land = null;
            LandData landData = list[i];

            land.transform.SetParent(root);
            land.transform.localPosition = new Vector3(landData.x, 0, landData.z);
            instanceMapObjectList.Add(land.gameObject);
            log += $"landType = {landData.landType}, x = {landData.x} , z = {landData.z}, \n";
        }

        // �Ŀ� navmesh���� �۾� �ʿ�
        Debug.LogError(log);
    }
    public void ClearMap()
    {

    }

}
