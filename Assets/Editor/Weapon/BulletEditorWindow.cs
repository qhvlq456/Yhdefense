#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
public class BulletEditorWindow : EditorWindow
{
    private List<WeaponData> weaponList = new();
    private Vector2 scrollPos;

    [MenuItem("Tools/Weapon/Bullet Editor")]
    public static void Open()
    {
        GetWindow<BulletEditorWindow>("Weapon Editor");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Add New Weapon"))
        {
            weaponList.Add(new WeaponData
            {
                index = 0,
                speed = 1,
                name = "NewWeapon",
                weaponType = WeapondType.bullet,
            });
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < weaponList.Count; i++)
        {
            GUILayout.BeginVertical("box");

            weaponList[i] = new WeaponData
            {
                index = EditorGUILayout.IntField("Index", weaponList[i].index),
                speed = EditorGUILayout.FloatField("Speed", weaponList[i].speed),
                name = EditorGUILayout.TextField("Name", weaponList[i].name),
                weaponType = (WeapondType)EditorGUILayout.EnumPopup("Weapon Type", weaponList[i].weaponType),
            };

            if (GUILayout.Button("Remove Weapon"))
            {
                weaponList.RemoveAt(i);
                GUILayout.EndVertical();  // 반드시 먼저 EndVertical 호출
                break;
            }

            GUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save JSON"))
            SaveJson();

        if (GUILayout.Button("Load JSON"))
            LoadJson();
        EditorGUILayout.EndHorizontal();
    }

    private void SaveJson()
    {
        string path = EditorUtility.SaveFilePanel("Save Weapon JSON", Application.dataPath, "WeaponData", "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = JsonConvert.SerializeObject(weaponList, Formatting.Indented);
            File.WriteAllText(path, json);
            Debug.Log("Saved Weapon JSON to " + path);
        }
    }

    private void LoadJson()
    {
        string path = EditorUtility.OpenFilePanel("Load Weapon JSON", Application.dataPath, "json");
        if (!string.IsNullOrEmpty(path))
        {
            string json = File.ReadAllText(path);
            weaponList = JsonConvert.DeserializeObject<List<WeaponData>>(json);
            Debug.Log("Loaded Weapon JSON from " + path);
        }
    }
}
#endif