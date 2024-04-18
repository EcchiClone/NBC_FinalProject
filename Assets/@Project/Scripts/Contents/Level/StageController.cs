using System.Collections;
using UnityEngine;

public class StageController : MonoBehaviour
{
    private SpawnManager _spawnManager;
    private ObstacleSpawner _obstacleManager;
    
    public void GameStart()
    {
        Managers.ActionManager.OnPlayerDead += GameOver;

        _spawnManager = Managers.SpawnManager;
        _obstacleManager = GetComponent<ObstacleSpawner>();
        _obstacleManager.SpawnObstacle();

        StartCoroutine(_spawnManager.Co_TimerOn());
        StartCoroutine(_spawnManager.GameStart());
    }

    private void GameOver()
    {
        StopAllCoroutines();
        Managers.UI.ShowPopupUI<UI_ResultPopup>();
    }
}