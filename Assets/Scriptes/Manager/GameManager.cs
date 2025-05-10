using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int selectStage;

    [SerializeField]
    private int life;

    private IEnumerator GameFlowRoutine = null;

    private void StartGame(int _stage, int _subStage)
    {
        selectStage = _stage;
        MapManager.Instance.SetMap(_stage, _subStage);


    }
    
    private void EndGame()
    {

    }
    private IEnumerator CoGameFlow()
    {
        yield return null;
    }
}
