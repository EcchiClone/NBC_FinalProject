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
        Managers.SpawnManager.GameOver();
    }

    private void OnDrawGizmos()
    {
        if (Managers.SpawnManager._groundTempPoint.Count <= 0)
            return;
        Gizmos.color = Color.green;
        foreach (Vector3 point in Managers.SpawnManager._groundTempPoint)
        {
            Gizmos.DrawSphere(point, 1f);
        }
    }
}