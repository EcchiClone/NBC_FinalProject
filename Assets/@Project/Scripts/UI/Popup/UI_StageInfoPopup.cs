using TMPro;
using UnityEngine;

public class UI_StageInfoPopup : UI_Popup
{
    [Header("Stage")]
    [SerializeField] TextMeshProUGUI _currentStage;
    [SerializeField] TextMeshProUGUI _countDownTime;
    [SerializeField] TextMeshProUGUI _spawnedCount;
    [SerializeField] TextMeshProUGUI _remainCount;

    protected override void Init()
    {
        base.Init();

        Managers.StageActionManager.OnEnemySpawned += CountSpawndEnemies;
        Managers.SpawnManager.OnUpdateEnemyKillCount += CountRemainEnemies;
        Managers.StageActionManager.OnCountDownActive += CountDownTime;
        Managers.SpawnManager.OnStageClear += StageClear;

        Managers.ActionManager.OnPlayerDead += () => gameObject.SetActive(false);
    }

    private void CountSpawndEnemies(int value)
    {
        _spawnedCount.text = $"소환된 적 : {value}";
    }

    private void CountRemainEnemies()
    {
        _remainCount.text = $"처치한 적 : {Managers.SpawnManager.MinionKillCount + Managers.SpawnManager.BossKillCount}";
    }

    private void CountDownTime(float remianTime)
    {
        if (remianTime <= 0.1)
        {
            _countDownTime.text = "";
            return;
        }

        int minutes = (int)remianTime / 60;
        int seconds = (int)remianTime % 60;

        if (Managers.SpawnManager.LevelData.EnemyType == Define.EnemyType.Minion)
            _countDownTime.text = $"다음 웨이브까지 : {minutes:00}:{seconds:00}";
        else if (Managers.SpawnManager.LevelData.EnemyType == Define.EnemyType.Boss)
            _countDownTime.text = $"<color=red>남은 시간</color> : {minutes:00}:{seconds:00}";
    }

    private void StageClear()
    {
        _countDownTime.text = "";
        _currentStage.text = $"현재 스테이지 : {Managers.SpawnManager.CurrentStage}";
    }
}
