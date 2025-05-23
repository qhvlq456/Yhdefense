#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

public class MapEditorWindow : EditorWindow
{
    public class LandIndexPair
    {
        public int idx;
        public LandType type;

        public LandIndexPair() { }
        public LandIndexPair(int _idx, LandType _type)
        {
            idx = _idx;
            type = _type;
        }

        public Color GetColor()
        {
            return type switch
            {
                LandType.hero => Color.green,
                LandType.enemy => Color.red,
                LandType.deco => Color.gray,
                _ => Color.white
            };
        }

        public LandType NextType()
        {
            return type switch
            {
                LandType.hero => LandType.enemy,
                LandType.enemy => LandType.deco,
                _ => LandType.hero
            };
        }
    }

    private const int gridSize = 10;
    private LandIndexPair[,] grid = new LandIndexPair[gridSize, gridSize];

    private int stageIndex = 0;
    private int subStageIndex = 0;
    private Vector3 startPoint = Vector3.zero;
    private Vector3 endPoint = Vector3.zero;

    private int life = 10;

    private List<int> subStageIdxList = new();
    private List<StageData> stageDataList = new();
    private Vector2 scrollPos;

    [MenuItem("Tools/Map/Map Editor")]
    public static void OpenWindow()
    {
        GetWindow<MapEditorWindow>("Map Editor");
    }

    private void OnEnable()
    {
        for (int z = 0; z < gridSize; z++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                if (grid[z, x] == null)
                    grid[z, x] = new LandIndexPair();
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Stage Configuration", EditorStyles.boldLabel);
        stageIndex = EditorGUILayout.IntField("Stage Index", stageIndex);
        startPoint = EditorGUILayout.Vector3Field("Start Point", startPoint);
        endPoint = EditorGUILayout.Vector3Field("End Point", endPoint);
        life = EditorGUILayout.IntField("Life", life);

        GUILayout.Space(10);
        GUILayout.Label("Land Type Grid");

        // 위에서 아래로 그리는 형식이기 때문에 --
        for (int z = gridSize - 1; z >= 0; z--)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < gridSize; x++)
            {
                if (startPoint.x == x && startPoint.z == z)
                {
                    GUI.backgroundColor = Color.blue;
                }
                else if (endPoint.x == x && endPoint.z == z)
                {
                    GUI.backgroundColor = Color.black;
                }
                else
                {
                    GUI.backgroundColor = grid[z, x].GetColor();
                }

                Rect rect = GUILayoutUtility.GetRect(30, 30);
                if (GUI.Button(rect, grid[z, x].idx.ToString()))
                {
                    if (Event.current.button == 1) // Right Click
                    {
                        grid[z, x].type = grid[z, x].NextType();
                        Repaint();
                    }
                    else if (Event.current.button == 0) // Left Click
                    {
                        // 람다식은 나중에 callback되기 때문에 for문이 모두 동작하여 x,z 값이 10으로 변하는 현상이 발생함으로 명시적으로 
                        // 매개변수를 전달하기 위해 captured 변수 선언
                        int capturedX = x;
                        int capturedZ = z;

                        IndexInputPopup.Open(capturedX, capturedZ, grid[capturedZ, capturedX].idx, 
                        (newIdx) =>
                        {
                            Debug.LogError($"OnIndexed x : {capturedX} ,. z : {capturedZ}");
                            grid[capturedZ, capturedX].idx = newIdx;
                        },
                        () => 
                        {
                            Debug.LogError($"OnStartPointed x : {capturedX} ,. z : {capturedZ}");
                            startPoint = new Vector3(capturedX, 0, capturedZ);
                            grid[capturedZ, capturedX].type = LandType.enemy;
                        }
                        ,
                        () => 
                        {
                            Debug.LogError($"OnEndPointed x : {capturedX} ,. z : {capturedZ}");
                            endPoint = new Vector3(capturedX, 0, capturedZ);
                            grid[capturedZ, capturedX].type = LandType.enemy;
                        }
                        );
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        GUI.backgroundColor = Color.white;

        GUILayout.Space(20);
        DrawSubStageEditor();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Stage To List"))
        {
            AddCurrentStageToList();
        }

        if (GUILayout.Button("Clear Stage List"))
        {
            if (EditorUtility.DisplayDialog("Clear All?", "모든 스테이지 데이터가 초기화됩니다. 계속할까요?", "확인", "취소"))
            {
                stageDataList.Clear();
            }
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save JSON"))
        {
            SaveJson();
        }

        if (GUILayout.Button("Load JSON"))
        {
            LoadJson();
        }
        GUILayout.EndHorizontal();
    }

    private void DrawSubStageEditor()
    {
        GUILayout.Label("Sub Stage Editor", EditorStyles.boldLabel);

        subStageIndex = EditorGUILayout.IntField("subStage Index", subStageIndex);

        if (GUILayout.Button("Add SubStage"))
        {
            subStageIdxList.Add(subStageIndex);
        }

        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(200));

        for (int i = 0; i < subStageIdxList.Count; i++)
        {
            GUILayout.BeginVertical("box");
            GUILayout.Label($"List index : {i}, SubStageIdx : {subStageIdxList[i]}");

            if (GUILayout.Button("Remove SubStage"))
            {
                subStageIdxList.RemoveAt(i);
                i--;
            }

            GUILayout.EndVertical();
        }

        GUILayout.EndScrollView();
    }

    private void AddCurrentStageToList()
    {
        // 의도치 않게 정확히 offset 1단위로 우상단으로 올라가게 설정
        List<LandData> landList = new();
        for (int z = 0; z < gridSize; z++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                landList.Add(new LandData
                {
                    index = grid[z, x].idx,
                    x = x,
                    z = z,
                    landType = grid[z, x].type
                });
            }
        }

        StageData stage = new StageData
        {
            index = stageIndex,
            life = life,
            startPoint = startPoint,
            endPoint = endPoint,
            landDataList = landList,
            subStageIdxList = new List<int>(subStageIdxList)
        };

        stageDataList.Add(stage);

        // 초기화
        stageIndex++;
        subStageIdxList.Clear();
        grid = new LandIndexPair[gridSize, gridSize];
        OnEnable();
    }

    private void SaveJson()
    {
        if (stageDataList.Count == 0)
        {
            Debug.LogWarning("저장할 스테이지가 없습니다.");
            return;
        }

        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.Converters.Add(new Vector2Converter());
        settings.Converters.Add(new Vector3Converter());

        string json = JsonConvert.SerializeObject(stageDataList, settings);
        string path = EditorUtility.SaveFilePanel("Save Stage List JSON", Application.dataPath, "MapData", "json");
        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, json);
            Debug.Log($"Stage list saved to: {path}");
        }
    }

    private void LoadJson()
    {
        string path = EditorUtility.OpenFilePanel("Load Stage JSON", Application.dataPath, "json");
        if (!string.IsNullOrEmpty(path))
        {
            try
            {
                string json = File.ReadAllText(path);
                stageDataList = JsonConvert.DeserializeObject<List<StageData>>(json);
                int idx = stageDataList.FindIndex(x => x.index == stageIndex);

                if (idx == -1)
                {
                    Debug.LogError($"Load failed: not exist stage index");
                }
                else
                {
                    ApplyStageToEditor(stageDataList[idx]);
                    Debug.Log($"Loaded {stageDataList.Count} stages from file.");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Load failed: {ex.Message}");
            }
        }
    }
    private void ApplyStageToEditor(StageData _stage)
    {
        stageIndex = _stage.index;
        life = _stage.life;
        subStageIdxList = new List<int>(_stage.subStageIdxList);

        // Grid 초기화
        grid = new LandIndexPair[gridSize, gridSize];
        for (int z = 0; z < gridSize; z++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                grid[z, x] = new LandIndexPair(); // 기본값 초기화
            }
        }

        // LandData 반영
        foreach (var land in _stage.landDataList)
        {
            if (land.x >= 0 && land.x < gridSize && land.z >= 0 && land.z < gridSize)
            {
                grid[land.z, land.x] = new LandIndexPair(land.index, land.landType);
            }
        }

        Repaint(); // 에디터 새로고침
    }

}
#endif
