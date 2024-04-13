using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Stage
{
    Stage1,
    Stage2,
    BossStage
}


public class StageController : MonoBehaviour
{
    private float _stageTimer = 0f;
    private float _limitTime = 30f;

    private Stage _currentStage = Stage.Stage1;
    private int _currentStageNumber = 1;

    SpawnManager _spawnManager;
    ObstacleSpawner _obstacleManager;

    void Start()
    {
        _spawnManager = Managers.SpawnManager;

        _obstacleManager = GetComponent<ObstacleSpawner>();

        StartStage();
    }

    void Update()
    {
        _stageTimer -= Time.deltaTime;
        switch (_currentStage)
        {
            case Stage.Stage1:
            case Stage.Stage2:
                if (_stageTimer > 0 && _spawnManager.AllUnitDisabled())
                    StageClear();
                else if (_stageTimer <= 0 && !_spawnManager.AllUnitDisabled())
                    GameOver();
                break;

            case Stage.BossStage:
                if (_stageTimer > 0 && _spawnManager.AllUnitDisabled())
                    StageClear();
                else if (_stageTimer <= 0 && !_spawnManager.AllUnitDisabled())
                    GameOver();
                break;
        }
    }

    public void StartStage()
    {
        Debug.Log("현재 스테이지 : " + _currentStageNumber);
        switch (_currentStage)
        {
            case Stage.Stage1:
                _stageTimer = 10f;
                _obstacleManager.SpawnObstacle();   // 2. 지형 생성                
                _spawnManager.SpawnUnits(
                    new List<UnitSpawnInfo>
                    {
                        new UnitSpawnInfo(UnitType.Minion_Spider, 20),
                        new UnitSpawnInfo(UnitType.Minion_Ball, 20),
                    }
                    );    // 3. 적 스폰
                break;

            case Stage.Stage2:
                _stageTimer = 10f;
                _spawnManager.SpawnUnits(
                    new List<UnitSpawnInfo>
                    {
                        new UnitSpawnInfo(UnitType.Minion_Spider, 30),
                        new UnitSpawnInfo(UnitType.Minion_Ball, 30),
                    }
                    );
                break;

            case Stage.BossStage:
                _stageTimer = 10f;
                _obstacleManager.RemoveObstacle();
                _spawnManager.SpawnBoss();
                _spawnManager.SpawnUnits(
                    new List<UnitSpawnInfo>
                    {
                        new UnitSpawnInfo(UnitType.Boss_SkyFire, 1)
                    }
                    );
                break;
        }
    }

    public void StageClear()
    {
        ++_currentStageNumber;
        _currentStage = (Stage)(_currentStageNumber % 3);
        StartStage();
    }

    private void GameOver()
    {
        // 게임 오버 씬으로        
        Debug.Log("게임 오버");

        // 임시로 무한으로 돌도록 함
        _currentStageNumber = 0;
        _currentStage = Stage.Stage1;
        StartStage();
    }

    // 스테이지 넘버에 따라서 


    //#region 국 작업        
    //private float _timer;

    //private Coroutine co_TimerRoutine;

    //public void Init()
    //{
    //    _spawnManager = Managers.SpawnManager;
    //    _spawnManager.OnStageClear += () => _timer = 0;
    //    Managers.ActionManager.OnPlayerDead += StopTimer;

    //    _obstacleManager = GetComponent<ObstacleSpawner>();
    //    _obstacleManager.SpawnObstacle();        

    //    _spawnManager.StageStart();
    //    co_TimerRoutine = StartCoroutine(Co_StartTimer());
    //}

    //private IEnumerator Co_StartTimer()
    //{
    //    while (true)
    //    {
    //        _timer += Time.deltaTime;
    //        if (_timer > _spawnManager.LevelData.SpawnTime)
    //            break;            

    //        yield return null;
    //    }
    //    _spawnManager.TimeOut();
    //}

    //private void StopTimer()
    //{
    //    if (co_TimerRoutine != null)
    //        StopCoroutine(co_TimerRoutine);

    //    co_TimerRoutine = null;
    //}
    //#endregion
}