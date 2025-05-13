using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Game/CharacterResData")]
public class CharacterResData : ScriptableObject
{
    public int index;
    public GameObject prefab;
}
