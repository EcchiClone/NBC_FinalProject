using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;




public class SpawnManager
{
    public List<Vector3> _groundCell = new List<Vector3>(); // 탐지 전의 모든 지상 셀 (어떤 요소와도 겹치지 않음)

    public HashSet<Vector3> _groundTempPoint = new HashSet<Vector3>(); // 탐지 후 중복 요소 제거용

    public Queue<Vector3> _groundSpawnPoints = new Queue<Vector3>(); // 탐지 후 찾아낸 스폰 포인트

    public Vector3 gridWorldSize; // 맵의 3차원 크기
    public float cellRadius;
    int gridSizeX, gridSizeY, gridSizeZ;
    private float _cellDiameter;

    private List<GameObject> _activatedUnits = new List<GameObject>();

    public SpawnManager()
    {
        gridWorldSize = new Vector3(200, 200, 200);
        cellRadius = 5;

        _cellDiameter = cellRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / _cellDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / _cellDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / _cellDiameter);
    }

    private void DestroyAllUnit()
    {
        if (_activatedUnits.Count > 0)
        {
            foreach (GameObject obj in _activatedUnits)
            {
                obj.SetActive(false);
            }
            _activatedUnits.Clear();
        }
    }

    public bool AllUnitDisabled()
    {
        if (_activatedUnits.Count == 0) return true; // 임시

        foreach (GameObject unit in _activatedUnits)
        {
            if (unit.activeSelf) return false;
        }

        return true;
    }

    private Vector3 GetSpawnPoint()
    {
        Vector3 spawnPoint = _groundSpawnPoints.Dequeue();
        _groundSpawnPoints.Enqueue(spawnPoint);

        return spawnPoint;
    }

    #region 스폰 포인트 연산
    public void CreateCell() // 스폰 가능한 위치 선정
    {
        _groundCell.Clear();
        Vector3 wolrdBottomLeft = Vector3.zero - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.z / 2;

        for (int x = 0; x < gridSizeX; ++x)
        {
            for (int z = 0; z < gridSizeZ; ++z)
            {
                Vector3 groundSpawnPoint =
                    wolrdBottomLeft +
                    Vector3.right * (x * _cellDiameter + cellRadius) +
                    Vector3.up * 40 +
                    Vector3.forward * (z * _cellDiameter + cellRadius);

                if (!Physics.CheckSphere(groundSpawnPoint, cellRadius))
                {
                    _groundCell.Add(groundSpawnPoint);
                }
            }
        }

        DetectSpawnPoint();
        ShuffleSpawnPoint();
    }

    public void DetectSpawnPoint() // 중복 없는 스폰 포인트 탐지
    {
        _groundTempPoint.Clear();
        _groundSpawnPoints.Clear();

        int groundLayer = LayerMask.GetMask("Ground");
        int allLayers = LayerMask.GetMask("Ground", "Wall", "Unwalkable");

        RaycastHit hit;
        foreach (Vector3 cell in _groundCell)
        {
            if (Physics.Raycast(cell, Vector3.down, out hit, 41f, groundLayer))
            {
                Collider[] hitColliders = Physics.OverlapSphere(hit.point, cellRadius, allLayers);
                bool onlyGround = true;
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.gameObject.layer != LayerMask.NameToLayer("Ground"))
                    {
                        onlyGround = false;
                        break;
                    }
                }
                if (onlyGround)
                {
                    _groundTempPoint.Add(hit.point + Vector3.up * 3);
                }
            }       
        }

        _groundSpawnPoints = new Queue<Vector3>(_groundTempPoint);
    }

    public void ShuffleSpawnPoint() // 유틸에 넣기
    {
        if (_groundSpawnPoints.Count <= 0)
            return;

        List<Vector3> tempList = new List<Vector3>(_groundSpawnPoints);

        System.Random random = new System.Random();

        int n = _groundSpawnPoints.Count;

        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            Vector3 value = tempList[k];
            tempList[k] = tempList[n];
            tempList[n] = value;
        }

        _groundSpawnPoints = new Queue<Vector3>(tempList);
    }
    #endregion



    #region 국 작업
    public event Action OnStageClear;
    public event Action OnUpdateEnemyKillCount;        

    public StageData StageData { get; private set; }
    public LevelData LevelData { get; private set; }

    public int CurrentSpawnCount { get; private set; }
    public bool IsStarted { get; private set; }

    public float Timer { get => StageData.time; private set { StageData.time = value; } }
    public int CurrentStage { get => StageData.stage; private set { StageData.stage = value;} }
    public int MinionKillCount { get => StageData.minionKill; private set { StageData.minionKill = value; OnUpdateEnemyKillCount?.Invoke(); } }
    public int BossKillCount { get => StageData.bossKill; private set { StageData.bossKill = value; OnUpdateEnemyKillCount?.Invoke(); _bossClear = true;} }
    public int ResearchPoint { get => StageData.researchPoint; private set { StageData.researchPoint = value;} }

    private int _currentWaveSpawnCount;
    private float _timer;
    private bool _bossClear;

    public void Init()
    {
        StageData = new StageData();

        Managers.StageActionManager.OnMinionKilled += () => MinionKillCount++;
        Managers.StageActionManager.OnBossKilled += () => BossKillCount++;
        Managers.ActionManager.OnPlayerDead += () => IsStarted = false;
    }

    public IEnumerator Co_TimerOn()
    {
        while (true)
        {
            Timer += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator GameStart()
    {
        if (!IsStarted)
        {
            IsStarted = true;
            CurrentStage = 1;
        }
        int levelCount = Managers.Data.GetLevelDataDictCount();
        CreateCell();

        while (CurrentStage <= levelCount)
        {
            LevelData = Managers.Data.GetLevelData(CurrentStage);

            yield return CoroutineManager.StartCoroutine(Co_SpawnEnemies());
            yield return CoroutineManager.StartCoroutine(Co_StartCountDown());
            if (LevelData.EnemyType == EnemyType.Boss && !_bossClear)
                break;
            if (!IsStarted)
                yield break;
            NextStage();            
        }
        TimeOut();
    }

    public IEnumerator Co_SpawnEnemies()
    {
        while (true)
        {
            if (!IsStarted)
                yield break;
            
            yield return Util.GetWaitSeconds(LevelData.SpawnDelayTime);

            string unitType = LevelData.SpawnTypes[UnityEngine.Random.Range(0, LevelData.SpawnTypes.Count)].ToString();
            SpawnEnemy(unitType);

            if (_currentWaveSpawnCount >= LevelData.SpawnCount)
                break;
        }
    }

    public IEnumerator Co_StartCountDown()
    {
        _timer = LevelData.CountDownTime;

        while (_timer > 0)
        {
            if (!IsStarted || _bossClear)
                yield break;

            _timer -= Time.deltaTime;
            Managers.StageActionManager.CallCountDown(_timer);
            yield return null;
        }
    }

    private void NextStage()
    {
        _currentWaveSpawnCount = 0;
        _bossClear = false;
        CurrentStage++;
        OnStageClear?.Invoke();
    }

    public void SpawnEnemy(string unitType)
    {
        _currentWaveSpawnCount++;
        CurrentSpawnCount++;

        if (LevelData.EnemyType == EnemyType.Minion)
            ObjectPooler.SpawnFromPool(unitType, GetSpawnPoint());
        else if (LevelData.EnemyType == EnemyType.Boss)
        {
            BGMPlayer.Instance.SetFieldBGMState(1f); // 보스 브금 재생
            ObjectPooler.SpawnFromPool(unitType, new Vector3(UnityEngine.Random.Range(0f, 50f), 50f, UnityEngine.Random.Range(0f, 50f)));
        }
        Managers.StageActionManager.CallEnemySpawned(CurrentSpawnCount);
    }

    public bool CheckStageClear()
    {
        if (_bossClear)
            return true;
        return false;
    }

    public void TimeOut()
    {
        IsStarted = false;
        Managers.ActionManager.CallPlayerDead();
    }

    public void GameOver()
    {
        StageData.researchPoint = CurrentStage * 2;
        Managers.UI.ShowPopupUI<UI_ResultPopup>();
    }
    #endregion

    public void Clear()
    {        
        CurrentSpawnCount = 0;
        _currentWaveSpawnCount = 0;        

        StageData = null;
        OnStageClear = null;
        OnUpdateEnemyKillCount = null;        
    }
}