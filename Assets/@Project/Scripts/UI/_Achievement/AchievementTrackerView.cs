using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AchievementTrackerView : MonoBehaviour
{
    [SerializeField]
    private AchievementTracker achievementTrackerPrefab;
    [SerializeField]
    private CategoryColor[] categoryColors;

    private void Start()
    {
        AchievementSystem.Instance.onAchievementRegistered += CreateAchievementTracker;

        foreach (var achievement in AchievementSystem.Instance.ActiveAchievements)
            CreateAchievementTracker(achievement);
    }

    private void OnDestroy()
    {
        if (AchievementSystem.Instance)
            AchievementSystem.Instance.onAchievementRegistered -= CreateAchievementTracker;
    }

    private void CreateAchievementTracker(Achievement achievement)
    {
        var categoryColor = categoryColors.FirstOrDefault(x => x.category == achievement.Category);
        var color = categoryColor.category == null ? Color.white : categoryColor.color;
        Instantiate(achievementTrackerPrefab, transform).Setup(achievement, color);
    }

    [System.Serializable]
    private struct CategoryColor
    {
        public TaskCategory category;
        public Color color;
    }
}