using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_AchievementItem : UI_Item
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _rewardIcon;
    [SerializeField] private TextMeshProUGUI _rewardNum;
    [SerializeField] private TextMeshProUGUI _desc;
    [SerializeField] private TextMeshProUGUI _completeResult;
    [SerializeField] private Button _completeBtn;
    protected override void Init()
    {
        base.Init();
    }
    
    public void SetAchievementInfo(Achievement achievement)
    {
        _icon.sprite = achievement.Icon;
        try { _rewardIcon.sprite = achievement.Rewards[0].Icon; } catch { }
        try { _rewardNum.text = achievement.Rewards[0].Quantity.ToString(); } catch { }
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
                AchievementSystem.Instance.ReceiveRewardsAndCompleteAchievement(achievement.CodeName);
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
