using System;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public PlayerData playerData { private set; get; }
    private Action<int> OnAddCoin;

}
