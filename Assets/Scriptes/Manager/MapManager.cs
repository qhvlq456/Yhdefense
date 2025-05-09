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
    /// 무조건 0,0 에서부터 양수로 이동 후 카메라 중앙에 맞출것
    /// </summary>
    /// <param name="_stage">유저가 선택한 스테이지</param>
    public void SetMap(int _stage)
    {

    }
    public void ClearMap()
    {

    }

}
