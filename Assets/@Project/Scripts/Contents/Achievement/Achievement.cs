using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

using Debug = UnityEngine.Debug;

public enum AchievementState
{
    Inactive,
    Running,
    Complete,
    Cancel,
    WaitingForCompletion
}

[CreateAssetMenu(menuName = "Achievement/Achievement", fileName = "Achievement_")]
public class Achievement : ScriptableObject
{
    #region Events
    public delegate void TaskSuccessChangedHandler(Achievement achievement, AchievementTask task, int currentSuccess, int prevSuccess);
    public delegate void CompletedHandler(Achievement achievement);
    public delegate void CanceledHandler(Achievement achievement);
    public delegate void NewTaskGroupHandler(Achievement achievement, AchievementTaskGroup currentTaskGroup, AchievementTaskGroup prevTaskGroup);
    #endregion

    [SerializeField]
    private TaskCategory category;
    [SerializeField]
    private Sprite icon;

    [Header("Text")]
    [SerializeField]
    private string codeName;
    [SerializeField]
    private string displayName;
    [SerializeField, TextArea]
    private string description;

    [Header("Task")]
    [SerializeField]
    private AchievementTaskGroup[] taskGroups;

    [Header("Reward")]
    [SerializeField]
    private AchievementReward[] rewards;

    [Header("Option")]
    [SerializeField]
    private bool useAutoComplete;
    [SerializeField]
    private bool isCancelable;
    [SerializeField]
    private bool isSavable;

    [Header("Condition")]
    [SerializeField]
    private AchievementCondition[] acceptionConditions;
    [SerializeField]
    private AchievementCondition[] cancelConditions;

    private int currentTaskGroupIndex;

    public TaskCategory Category => category;
    public Sprite Icon => icon;
    public string CodeName => codeName;
    public string DisplayName => displayName;
    public string Description => description;
    public AchievementState State { get; private set; }
    public AchievementTaskGroup CurrentTaskGroup => taskGroups[currentTaskGroupIndex];
    public IReadOnlyList<AchievementTaskGroup> TaskGroups => taskGroups;
    public IReadOnlyList<AchievementReward> Rewards => rewards;
    public bool IsRegistered => State != AchievementState.Inactive;
    public bool IsComplatable => State == AchievementState.WaitingForCompletion;
    public bool IsComplete => State == AchievementState.Complete;
    public bool IsCancel => State == AchievementState.Cancel;
    public virtual bool IsCancelable => isCancelable && cancelConditions.All(x => x.IsPass(this));
    public bool IsAcceptable => acceptionConditions.All(x => x.IsPass(this));
    public virtual bool IsSavable => isSavable;

    public event TaskSuccessChangedHandler onTaskSuccessChanged;
    public event CompletedHandler onCompleted;
    public event CanceledHandler onCanceled;
    public event NewTaskGroupHandler onNewTaskGroup;

    public void OnRegister()
    {
        Debug.Assert(!IsRegistered, "This achievement has already been registered.");

        foreach (var taskGroup in taskGroups)
        {
            taskGroup.Setup(this);
            foreach (var task in taskGroup.Tasks)
                task.onSuccessChanged += OnSuccessChanged;
        }

        State = AchievementState.Running;
        CurrentTaskGroup.Start();
    }

    public void ReceiveReport(string category, object target, int successCount)
    {
        Debug.Assert(IsRegistered, "This achievement has already been registered.");
        Debug.Assert(!IsCancel, "This achievement has been canceled.");

        if (IsComplete)
            return;

        CurrentTaskGroup.ReceiveReport(category, target, successCount);

        if (CurrentTaskGroup.IsAllTaskComplete)
        {
            if (currentTaskGroupIndex + 1 == taskGroups.Length)
            {
                State = AchievementState.WaitingForCompletion;
                if (useAutoComplete)
                    Complete();
            }
            else
            {
                var prevTasKGroup = taskGroups[currentTaskGroupIndex++];
                prevTasKGroup.End();
                CurrentTaskGroup.Start();
                onNewTaskGroup?.Invoke(this, CurrentTaskGroup, prevTasKGroup);
            }
        }
        else
            State = AchievementState.Running;
    }

    public void Complete()
    {
        CheckIsRunning();

        foreach (var taskGroup in taskGroups)
            taskGroup.Complete();

        State = AchievementState.Complete;

        foreach (var reward in rewards)
            reward.Give(this);

        onCompleted?.Invoke(this);

        onTaskSuccessChanged = null;
        onCompleted = null;
        onCanceled = null;
        onNewTaskGroup = null;
    }

    public virtual void Cancel()
    {
        CheckIsRunning();
        Debug.Assert(IsCancelable, "This achievement can't be canceled");

        State = AchievementState.Cancel;
        onCanceled?.Invoke(this);
    }

    public Achievement Clone()
    {
        var clone = Instantiate(this);
        clone.taskGroups = taskGroups.Select(x => new AchievementTaskGroup(x)).ToArray();

        return clone;
    }
    public AchievementSaveData ToSaveData()
    {
        return new AchievementSaveData
        {
            codeName = codeName,
            state = State,
            taskGroupIndex = currentTaskGroupIndex,
            taskSuccessCounts = CurrentTaskGroup.Tasks.Select(x => x.CurrentSuccess).ToArray()
        };
    }
    public void LoadFrom(AchievementSaveData saveData)
    {
        State = saveData.state;
        currentTaskGroupIndex = saveData.taskGroupIndex;
        for (int i = 0; i < currentTaskGroupIndex; i++)
        {
            var taskGroup = taskGroups[i];
            taskGroup.Start();
            taskGroup.Complete();
        }
        for (int i = 0; i < saveData.taskSuccessCounts.Length; i++)
        {
            CurrentTaskGroup.Start();
            CurrentTaskGroup.Tasks[i].CurrentSuccess = saveData.taskSuccessCounts[i];
        }
    }

    private void OnSuccessChanged(AchievementTask task, int currentSuccess, int prevSuccess)
        => onTaskSuccessChanged?.Invoke(this, task, currentSuccess, prevSuccess);

    [Conditional("UNITY_EDITOR")]
    private void CheckIsRunning()
    {
        Debug.Assert(IsRegistered, "This achievement has already been registered");
        Debug.Assert(!IsCancel, "This achievement has been canceled.");
        Debug.Assert(!IsComplete, "This achievement has already been completed");
    }
}
