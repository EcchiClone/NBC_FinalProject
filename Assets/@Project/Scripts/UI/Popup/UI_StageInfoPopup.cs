using TMPro;
using UnityEngine;

public class UI_StageInfoPopup : UI_Popup
{
    [Header("Stage")]
    [SerializeField] TextMeshProUGUI _countDownTime;
    [SerializeField] TextMeshProUGUI _spawnedCount;
    [SerializeField] TextMeshProUGUI _killCount;

    protected override void Init()
    {
        base.Init();

        Managers.StageActionManager.OnEnemySpawned += CountSpawndEnemies;
        Managers.StageActionManager.OnEnemyKilled += CountRemainEnemies;
        Managers.StageActionManager.OnCountDownActive += CountDownTime;
    }

    private void CountSpawndEnemies(int value)
    {
        _spawnedCount.text = $"{value}";
    }

    private void CountRemainEnemies(int value)
    {
        _killCount.text = $"{value}";
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

        _countDownTime.text = $"{minutes:00}:{seconds:00}";
    }
}
