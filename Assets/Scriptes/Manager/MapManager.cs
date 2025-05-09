using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>, ILoader
{
    const int MAX_X = 20;
    const int MAX_Z = 20;

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
    public void SetMap(int _stage)
    {

    }
    public void ClearMap()
    {

    }

}
