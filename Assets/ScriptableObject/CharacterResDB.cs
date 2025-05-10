using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game/CharacterResDB")]
public class CharacterResDB : ScriptableObject
{
    public List<CharacterResData> characterList;

    private Dictionary<int, GameObject> characterDic;

    public void Initialize()
    {
        characterDic = new Dictionary<int, GameObject>();
        foreach (var data in characterList)
        {
            if (!characterDic.ContainsKey(data.index))
                characterDic.Add(data.index, data.prefab);
        }
    }

    public GameObject GetPrefab(int _idx)
    {
        if (characterDic == null)
        {
            Initialize();
        }

        if (characterDic.TryGetValue(_idx, out var obj))
            return obj;

        Debug.LogWarning($"Prefab not found for index {_idx}");
        return null;
    }
}
