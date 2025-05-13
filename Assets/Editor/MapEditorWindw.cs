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
    private Vector2 startPoint = Vector2.zero;
    private Vector2 endPoint = Vector2.zero;

    private int life = 10;

    private List<SubStageData> subStages = new();
    private List<StageData> stageDataList = new();
    private Vector2 scrollPos;

    [MenuItem("Tools/Map Editor")]
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
        startPoint = EditorGUILayout.Vector2Field("Start Point", startPoint);
        endPoint = EditorGUILayout.Vector2Field("End Point", endPoint);
        life = EditorGUILayout.IntField("Life", life);

        GUILayout.Space(10);
        GUILayout.Label("Land Type Grid");

        // ������ �Ʒ��� �׸��� �����̱� ������ --
        for (int z = gridSize - 1; z >= 0; z--)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < gridSize; x++)
            {
                if (startPoint.x == x && startPoint.y == z)
                {
                    GUI.backgroundColor = Color.blue;
                }
                else if (endPoint.x == x && endPoint.y == z)
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
                        // ���ٽ��� ���߿� callback�Ǳ� ������ for���� ��� �����Ͽ� x,z ���� 10���� ���ϴ� ������ �߻������� ��������� 
                        // �Ű������� �����ϱ� ���� captured ���� ����
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
                            startPoint = new Vector2(capturedX, capturedZ);
                            grid[capturedZ, capturedX].type = LandType.enemy;
                        }
                        ,
                        () => 
                        {
                            Debug.LogError($"OnEndPointed x : {capturedX} ,. z : {capturedZ}");
                            endPoint = new Vector2(capturedX, capturedZ);
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
            if (EditorUtility.DisplayDialog("Clear All?", "��� �������� �����Ͱ� �ʱ�ȭ�˴ϴ�. ����ұ��?", "Ȯ��", "���"))
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

        if (GUILayout.Button("Add SubStage"))
        {
            subStages.Add(new SubStageData
            {
                index = subStages.Count,
                restTime = 30f,
                enemyDataList = new List<EnemyData> { new EnemyData { index = 0, dieGold = 10 } }
            });
        }

        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(200));

        for (int i = 0; i < subStages.Count; i++)
        {
            GUILayout.BeginVertical("box");
            GUILayout.Label($"SubStage {i}");

            SubStageData subStage = subStages[i];
            subStage.restTime = EditorGUILayout.FloatField("Rest Time", subStage.restTime);

            for (int j = 0; j < subStage.enemyDataList.Count; j++)
            {
                GUILayout.BeginHorizontal();
                var enemy = subStage.enemyDataList[j];
                enemy.index = EditorGUILayout.IntField("Enemy Index", enemy.index);
                enemy.dieGold = EditorGUILayout.IntField("Die Gold", enemy.dieGold);
                subStage.enemyDataList[j] = enemy;

                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    subStage.enemyDataList.RemoveAt(j);
                    j--;
                }
                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add Enemy"))
            {
                subStage.enemyDataList.Add(new EnemyData { index = 0, dieGold = 10 });
            }

            if (GUILayout.Button("Remove SubStage"))
            {
                subStages.RemoveAt(i);
                i--;
            }
            else
            {
                subStages[i] = subStage;
            }

            GUILayout.EndVertical();
        }

        GUILayout.EndScrollView();
    }

    private void AddCurrentStageToList()
    {
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
            subStageDataList = new List<SubStageData>(subStages)
        };

        stageDataList.Add(stage);

        // �ʱ�ȭ
        stageIndex++;
        subStages.Clear();
        grid = new LandIndexPair[gridSize, gridSize];
        OnEnable();
    }

    private void SaveJson()
    {
        if (stageDataList.Count == 0)
        {
            Debug.LogWarning("������ ���������� �����ϴ�.");
            return;
        }

        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.Converters.Add(new Vector2Converter());
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
        subStages = new List<SubStageData>(_stage.subStageDataList);

        // Grid �ʱ�ȭ
        grid = new LandIndexPair[gridSize, gridSize];
        for (int z = 0; z < gridSize; z++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                grid[z, x] = new LandIndexPair(); // �⺻�� �ʱ�ȭ
            }
        }

        // LandData �ݿ�
        foreach (var land in _stage.landDataList)
        {
            if (land.x >= 0 && land.x < gridSize && land.z >= 0 && land.z < gridSize)
            {
                grid[land.z, land.x] = new LandIndexPair(land.index, land.landType);
            }
        }

        Repaint(); // ������ ���ΰ�ħ
    }

}
#endif
