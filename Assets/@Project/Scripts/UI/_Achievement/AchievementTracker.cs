using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementTracker : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI achievementTitleText;
    [SerializeField]
    private TaskDescriptor taskDescriptorPrefab;

    private Dictionary<AchievementTask, TaskDescriptor> taskDesriptorsByTask = new Dictionary<AchievementTask, TaskDescriptor>();

    private Achievement targetAchievement;

    private void OnDestroy()
    {
        if (targetAchievement != null)
        {
            targetAchievement.onNewTaskGroup -= UpdateTaskDescriptos;
            targetAchievement.onCompleted -= DestroySelf;
        }

        foreach (var tuple in taskDesriptorsByTask)
        {
            var task = tuple.Key;
            task.onSuccessChanged -= UpdateText;
        }
    }

    public void Setup(Achievement targetAchievement, Color titleColor)
    {
        this.targetAchievement = targetAchievement;

        achievementTitleText.text = targetAchievement.Category == null ?
            targetAchievement.DisplayName :
            $"[{targetAchievement.Category.DisplayName}] {targetAchievement.DisplayName}";

        achievementTitleText.color = titleColor;

        targetAchievement.onNewTaskGroup += UpdateTaskDescriptos;
        targetAchievement.onCompleted += DestroySelf;

        var taskGroups = targetAchievement.TaskGroups;
        UpdateTaskDescriptos(targetAchievement, taskGroups[0]); 

        if (taskGroups[0] != targetAchievement.CurrentTaskGroup)
        {
            for (int i = 1; i < taskGroups.Count; i++)
            {
                var taskGroup = taskGroups[i];
                UpdateTaskDescriptos(targetAchievement, taskGroup, taskGroups[i - 1]);

                if (taskGroup == targetAchievement.CurrentTaskGroup)
                    break;
            }
        }
    }

    private void UpdateTaskDescriptos(Achievement achievement, AchievementTaskGroup currentTaskGroup, AchievementTaskGroup prevTaskGroup = null)
    {
        foreach (var task in currentTaskGroup.Tasks)
        {
            var taskDesriptor = Instantiate(taskDescriptorPrefab, transform);
            taskDesriptor.UpdateText(task);
            task.onSuccessChanged += UpdateText;

            taskDesriptorsByTask.Add(task, taskDesriptor);
        }

        if (prevTaskGroup != null)
        {
            foreach (var task in prevTaskGroup.Tasks)
            {
                var taskDesriptor = taskDesriptorsByTask[task];
                taskDesriptor.UpdateTextUsingStrikeThrough(task);
            }
        }
    }

    private void UpdateText(AchievementTask task, int currentSucess, int prevSuccess)
    {
        taskDesriptorsByTask[task].UpdateText(task);
    }

    private void DestroySelf(Achievement achievement)
    {
        Destroy(gameObject);
    }
}
