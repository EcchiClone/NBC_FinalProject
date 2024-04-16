using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    private ObstacleSpawner _obstacleManager;

    private LevelData _levelData;
    private int _currentLevel;
    private int _currentSpawnCount;
    private int _currentKilledCount;
    private float _timer;

    public int CurrentSpawnCount
    {
        get => _currentSpawnCount;
        set => FieldUpdator.UpdateValueAndNotify(ref _currentSpawnCount, value, Managers.StageActionManager.CallEnemySpawned);
    }

    public int CurrentKilledCount
    {
        get => _currentKilledCount;
        set => FieldUpdator.UpdateValueAndNotify(ref _currentKilledCount, value, Managers.StageActionManager.CallEnemyKilled);
    }

    public float Timer
    {
        get => _timer;
        set => FieldUpdator.UpdateValueAndNotify(ref _timer, value, Managers.StageActionManager.CallCountDown);
    }

    private Coroutine co_CurrentCoroutine;

    public void Start()
    {
        Managers.StageActionManager.OnEnemyKilled += KillCount;
        Managers.ActionManager.OnPlayerDead += StopTimer;        

        _obstacleManager = GetComponent<ObstacleSpawner>();
        _obstacleManager.SpawnObstacle();

        SetupLevelInfo();
    }

    private IEnumerator Co_SpawnEnemies()
    {
        while (true)
        {
            yield return Util.GetWaitSeconds(_levelData.SpawnDelayTime);

            string unitType = _levelData.SpawnTypes[Random.Range(0, _levelData.SpawnTypes.Count)].ToString();
            Managers.SpawnManager.SpawnEnemy(unitType, CurrentSpawnCount);
            CurrentSpawnCount++;

            if (CurrentSpawnCount >= _levelData.SpawnCount)
                break;
        }

        co_CurrentCoroutine = StartCoroutine(Co_StartCountDown());
    }

    private void SetupLevelInfo()
    {
        _currentLevel++;
        _levelData = Managers.Data.GetLevelData(_currentLevel);

        Managers.SpawnManager.CreateCell();

        co_CurrentCoroutine = StartCoroutine(Co_SpawnEnemies());
    }

    private IEnumerator Co_StartCountDown()
    {
        Timer = _levelData.CountDownTime;

        while (true)
        {

            Timer -= Time.deltaTime;
            if (Timer <= 0)
                break;

            yield return null;
        }

        CurrentSpawnCount = 0;
        StageClear();        

        // 버그 해결되면 다시 풀기
        //Managers.SpawnManager.TimeOut();
    }

    private void StopTimer()
    {
        if (co_CurrentCoroutine != null)
            StopCoroutine(co_CurrentCoroutine);

        co_CurrentCoroutine = null;
    }

    private void StageClear()
    {
        StopCoroutine(co_CurrentCoroutine);
        SetupLevelInfo();
    }

    private void KillCount(int value)
    {
        CurrentKilledCount++;
        if (CurrentKilledCount >= _levelData.SpawnCount)
            StageClear();
    }
}