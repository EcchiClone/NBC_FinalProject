using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StageType
{

}


public class StageController : MonoBehaviour
{
    private float _elapsedTime = 0f;
    private float _limitTime = 5f;

    private int _currentStage = 1;

    // 스테이지 매니저를 담을 참조 변수
    SpawnManager _spawnManager;
    ObstacleSpawner _obstacleManager;

    void Start()
    {
        _spawnManager = Managers.SpawnManager;
        _spawnManager.Initialize();

        _obstacleManager = GetComponent<ObstacleSpawner>();

        SetStage(_currentStage);
    }

    void Update()
    {
        _elapsedTime += Time.deltaTime;
        if(_elapsedTime >= _limitTime)
        {
            GameOver();
            ++_currentStage;
            _elapsedTime = 0;
            Debug.Log("current stage : "+_currentStage);
            SetStage(_currentStage);
        }
    }

    public void SetStage(int stage)
    {
        int num = stage % 3;
        if (num == 0) num = 3;

        switch (num)
        {
            case 1:
                // 1. 기존 유닛 삭제
                _spawnManager.DestroyAllUnit();
                // 2. 지형 생성
                _obstacleManager.SpawnObstacle();
                // 3. 적 스폰
                _spawnManager.Spawn(10, 0);
                break;
            case 2:
                _spawnManager.DestroyAllUnit();
                _spawnManager.Spawn(10, 0);
                break;
            case 3:
                _spawnManager.DestroyAllUnit();
                _obstacleManager.RemoveObstacle();
                break;
            default:
                _currentStage = 1;
                break;
        }        
    }

    public void Clear()
    {
        ++_currentStage;
    }

    private void GameOver()
    {
        // 게임 오버 씬으로
        //Debug.Log();
    }
}