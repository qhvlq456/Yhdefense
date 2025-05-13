using UnityEngine;
using UnityEditor;
using System;

public class IndexInputPopup : EditorWindow
{
    private Action<int> onIndexEntered;
    private Action onStartPointed;
    private Action onEndPointed;
    private int currentIndex;
    private int x, z;

    public static void Open(int _x, int _z, int _current, 
        Action<int> _callback, Action _startPointed = null, Action _onEndPointed = null)
    {
        IndexInputPopup window = CreateInstance<IndexInputPopup>();
        window.titleContent = new GUIContent($"Set Index ({_z},{_x})");
        window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 100);
        window.currentIndex = _current;
        window.x = _x;
        window.z = _z;
        window.onIndexEntered = _callback;
        window.onStartPointed = _startPointed;
        window.onEndPointed = _onEndPointed;
        window.ShowUtility();
    }

    private void OnGUI()
    {
        GUILayout.Label($"Index for ({z}, {x})", EditorStyles.boldLabel);
        currentIndex = EditorGUILayout.IntField("Index", currentIndex);

        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("StartPoint?"))
        {
            onStartPointed?.Invoke();
            Close();
        }
        if (GUILayout.Button("EndPoint?"))
        {
            onEndPointed?.Invoke();
            Close();
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Save"))
        {
            onIndexEntered?.Invoke(currentIndex);
            Close();
        }
        if (GUILayout.Button("Cancel"))
        {
            Close();
        }
        GUILayout.EndHorizontal();
    }
}
