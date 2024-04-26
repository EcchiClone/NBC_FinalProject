using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Localization.Settings;

public class UI_AchievementItem : UI_Item
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _rewardIcon;
    [SerializeField] private TextMeshProUGUI _rewardNum;
    [SerializeField] private TextMeshProUGUI _desc;
    [SerializeField] private TextMeshProUGUI _completeResult;
    [SerializeField] private Button _completeBtn;

    public CanvasGroup canvasGroup;
    public float fadeInDuration = 1.0f;

    protected override void Init()
    {
        base.Init();
    }
    private void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1f, fadeInDuration);
    }

    public void SetAchievementInfo(Achievement achievement)
    {
        _icon.sprite = achievement.Icon;

        if (achievement.Rewards[0] is RewardPart part)
        {
            try { _rewardIcon.sprite = Resources.Load<Sprite>(Managers.Data.GetPartData(achievement.Rewards[0].QuantityOrValue).Sprite_Path); } catch { }
            try { _rewardNum.text = ""; } catch { }
        }
        else
        {
            try { _rewardIcon.sprite = achievement.Rewards[0].Icon; } catch { }
            try { _rewardNum.text = achievement.Rewards[0].QuantityOrValue.ToString(); } catch { }
        }

        string TCodeName = $"ACHIEVEMENT-{achievement.CodeName}-NAME";
        string TCodeDesc = $"ACHIEVEMENT-{achievement.CodeName}-DESC";
        string TName = LocalizationSettings.StringDatabase.GetLocalizedString("Localization_Achievement Table", TCodeName, LocalizationSettings.SelectedLocale);
        string TDesc = LocalizationSettings.StringDatabase.GetLocalizedString("Localization_Achievement Table", TCodeDesc, LocalizationSettings.SelectedLocale);
        string TInProgress = LocalizationSettings.StringDatabase.GetLocalizedString("Localization Table", "Text-InProgress", LocalizationSettings.SelectedLocale);
        string TGetRewards = LocalizationSettings.StringDatabase.GetLocalizedString("Localization Table", "UI-GetRewards", LocalizationSettings.SelectedLocale);
        string TComplete = LocalizationSettings.StringDatabase.GetLocalizedString("Localization Table", "UI-Complete", LocalizationSettings.SelectedLocale);

        //_desc.text = $"[{achievement.DisplayName}] {achievement.Description}{CurrentTaskProgressText(achievement)}";
        _desc.text = $"[{TName}] {TDesc}{CurrentTaskProgressText(achievement)}";

        Dictionary<AchievementState,string> keyValuePairs = new Dictionary<AchievementState, string>(){
            {AchievementState.Running, TInProgress },
            {AchievementState.WaitingForCompletion, TGetRewards },
            {AchievementState.Complete, TComplete },
        };
        _completeResult.text = keyValuePairs[achievement.State];
        if (achievement.State == AchievementState.WaitingForCompletion)
        {
            _completeBtn.interactable = true;
            _completeBtn.onClick.AddListener(() => {
                Managers.AchievementSystem.ReceiveRewardsAndCompleteAchievement(achievement.CodeName);
                _completeResult.text = TComplete;
                _completeBtn.interactable = false;
            });
        }
    }
    string CurrentTaskProgressText(Achievement achievement)
    {
        string nowProgressText = "";
        foreach (AchievementTask task in achievement.CurrentTaskGroup.Tasks)
        {
            string TCodeDesc = $"TASK-{achievement.CodeName}-DESC";
            string TDesc = LocalizationSettings.StringDatabase.GetLocalizedString("Localization_Achievement Table", TCodeDesc, LocalizationSettings.SelectedLocale);

            if (task.NeedSuccessToComplete != 1)
                nowProgressText += $"\n  {TDesc} ( {task.CurrentSuccess} / {task.NeedSuccessToComplete} )";
            else
                nowProgressText += $"\n  {TDesc}";
        }
        return nowProgressText;
    }
}
