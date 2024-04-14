using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

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

        _desc.text = $"[{achievement.DisplayName}] {achievement.Description}{CurrentTaskProgressText(achievement)}";
        Dictionary<AchievementState,string> keyValuePairs = new Dictionary<AchievementState, string>(){
            {AchievementState.Running, "진행중" },
            {AchievementState.WaitingForCompletion, "보 상\n받 기" },
            {AchievementState.Complete, "완 료" },
        };
        _completeResult.text = keyValuePairs[achievement.State];
        if (achievement.State == AchievementState.WaitingForCompletion)
        {
            _completeBtn.interactable = true;
            _completeBtn.onClick.AddListener(() => {
                Managers.AchievementSystem.ReceiveRewardsAndCompleteAchievement(achievement.CodeName);
                _completeResult.text = "완 료";
                _completeBtn.interactable = false;
            });
        }
    }
    string CurrentTaskProgressText(Achievement achievement)
    {
        string nowProgressText = "";
        foreach (AchievementTask task in achievement.CurrentTaskGroup.Tasks)
        {
            if (task.NeedSuccessToComplete != 1)
                nowProgressText += $"\n  {task.Description} ( {task.CurrentSuccess} / {task.NeedSuccessToComplete} )";
            else
                nowProgressText += $"\n  {task.Description}";
        }
        return nowProgressText;
    }
}
