using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

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
        string translatedText = LocalizationSettings.StringDatabase.GetLocalizedString("Localization Table", $"Text-SpawnedEnemy", LocalizationSettings.SelectedLocale);
        //_spawnedCount.text = $"소환된 적 : {value}"; // Localizing 이전
        _spawnedCount.text = $"{translatedText} : {value}";
    }

    private void CountRemainEnemies()
    {
        string translatedText = LocalizationSettings.StringDatabase.GetLocalizedString("Localization Table", $"Text-KilledEnemy", LocalizationSettings.SelectedLocale);
        //_remainCount.text = $"처치한 적 : {Managers.SpawnManager.MinionKillCount + Managers.SpawnManager.BossKillCount}"; // Localizing 이전
        _remainCount.text = $"{translatedText} : {Managers.SpawnManager.MinionKillCount + Managers.SpawnManager.BossKillCount}";
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
        {

            string translatedText = LocalizationSettings.StringDatabase.GetLocalizedString("Localization Table", $"Text-NextWave", LocalizationSettings.SelectedLocale);
            //_countDownTime.text = $"다음 웨이브까지 : {minutes:00}:{seconds:00}"; // Localizing 이전
            _countDownTime.text = $"{translatedText} : {minutes:00}:{seconds:00}";
        }
        else if (Managers.SpawnManager.LevelData.EnemyType == Define.EnemyType.Boss)
        {

            string translatedText = LocalizationSettings.StringDatabase.GetLocalizedString("Localizationg Table", $"Text-Remain", LocalizationSettings.SelectedLocale);
            //_countDownTime.text = $"<color=red>남은 시간</color> : {minutes:00}:{seconds:00}"; // Localizing 이전
            _countDownTime.text = $"<color=red>{translatedText}</color> : {minutes:00}:{seconds:00}";
        }
    }

    private void StageClear()
    {
        string translatedText = LocalizationSettings.StringDatabase.GetLocalizedString("Localizationg Table", $"Text-CurrentStage", LocalizationSettings.SelectedLocale);
        _countDownTime.text = "";
        //_currentStage.text = $"현재 스테이지 : {Managers.SpawnManager.CurrentStage}"; // Localizing 이전
        _currentStage.text = $"{translatedText} : {Managers.SpawnManager.CurrentStage}";
    }
}
