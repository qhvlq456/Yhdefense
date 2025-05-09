using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int selectStage;

    [SerializeField]
    private int life;

    private IEnumerator GameFlowRoutine = null;

    private void StartGame(int _stage)
    {
        selectStage = _stage;
        MapManager.Instance.SetMap(_stage);


    }
    
    private void EndGame()
    {

    }
    private IEnumerator CoGameFlow()
    {
        yield return null;
    }
}
