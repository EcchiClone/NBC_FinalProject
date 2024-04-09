using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

using Debug = UnityEngine.Debug;

public enum AchievementState
{
    Inactive, // 비활성화 상태
    Running, // 진행중인 상태
    WaitingForCompletion, // 완료를 기다리는 상태(해당 방식 채택)
    Complete, // 완료 상태
    Cancel, // 중단 상태(구현 예정 없음)
}

[CreateAssetMenu(menuName = "Achievement/Achievement", fileName = "Achievement_")]
public class Achievement : ScriptableObject
{
    #region Events
    public delegate void TaskSuccessChangedHandler(Achievement achievement, AchievementTask task, int currentSuccess, int prevSuccess);
    public delegate void WaitingForCompletionHandler(Achievement achievement);
    public delegate void CompletedHandler(Achievement achievement);
    public delegate void CanceledHandler(Achievement achievement);
    public delegate void NewTaskGroupHandler(Achievement achievement, AchievementTaskGroup currentTaskGroup, AchievementTaskGroup prevTaskGroup);
    #endregion

    [Header("Text")]
    [SerializeField] 
    private string codeName;
    [SerializeField]
    private Sprite icon;
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

    // 부가옵션, 미사용 예정
    [Header("Option")]
    [SerializeField]
    private bool useAutoComplete; // 삭제... 했었으나 일단 되살림. 문제 없을듯.
    //[SerializeField]
    //private bool isCancelable; // 삭제
    //[SerializeField]
    //private bool isSavable;

    // 업적 활성화 조건과 업적퀘 중단(cancel) 조건
    //[Header("Condition")]
    //[SerializeField]
    //private AchievementCondition[] acceptionConditions;
    //[SerializeField]
    //private AchievementCondition[] cancelConditions;

    private AchievementState state;
    private int currentTaskGroupIndex;

    public string CodeName => codeName;
    public Sprite Icon => icon;
    public string DisplayName => displayName;
    public string Description => description;
    public AchievementState State{
        get
        {
            return state;
        }
        private set
        {
            state = value;
        }
    }
    public AchievementTaskGroup CurrentTaskGroup => taskGroups[currentTaskGroupIndex];
    public IReadOnlyList<AchievementTaskGroup> TaskGroups => taskGroups;
    public IReadOnlyList<AchievementReward> Rewards => rewards;
    public bool IsAutoComplete => useAutoComplete;
    public bool IsRegistered => State != AchievementState.Inactive;
    public bool IsWaitingForCompletion => State == AchievementState.WaitingForCompletion;
    public bool IsComplete => State == AchievementState.Complete;
    public bool IsCancel => State == AchievementState.Cancel;
    //public virtual bool IsCancelable => isCancelable && cancelConditions.All(x => x.IsPass(this));
    //public bool IsAcceptable => acceptionConditions.All(x => x.IsPass(this));
    //public virtual bool IsSavable => isSavable;

    public event TaskSuccessChangedHandler onTaskSuccessChanged;
    public event WaitingForCompletionHandler onWaitForComplete;
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

        if (IsComplete||IsWaitingForCompletion)
            return;

        CurrentTaskGroup.ReceiveReport(category, target, successCount);

        if (CurrentTaskGroup.IsAllTaskComplete)
        {
            if (currentTaskGroupIndex + 1 == taskGroups.Length)
            {
                //State = AchievementState.WaitingForCompletion;
                if (!useAutoComplete)
                    SetWaitForCompletion();
                else
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

    public void SetWaitForCompletion()
    {
        CheckIsRunning();

        foreach (var taskGroup in taskGroups)
            taskGroup.Complete();

        State = AchievementState.WaitingForCompletion;

        onWaitForComplete?.Invoke(this);
    }

    public void Complete()
    {
        CheckIsRunning();

        foreach (var taskGroup in taskGroups) // SetWaitForCompletion() 과 중복처리지만, 굳이 뺄 이유는 없어서 남겨둠
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

    public virtual void Cancel() // 퀘스트가 아니라 업적이므로 아무래도 미사용 예정
    {
        CheckIsRunning();
        //Debug.Assert(IsCancelable, "This achievement can't be canceled");

        State = AchievementState.Cancel;
        onCanceled?.Invoke(this);
    }

    public void ReceiveRewardsAndComplete()
    {
        if (State == AchievementState.WaitingForCompletion)
        {
            foreach (var reward in rewards)
            {
                reward.Give(this);
            }

            State = AchievementState.Complete;

            onCompleted?.Invoke(this);
        }
        else
        {
            Debug.LogWarning("업적이 완료 대기 상태가 아님. 버튼 미반응.");
        }
    }

    public Achievement Clone()
    {
        var clone = Instantiate(this);
        clone.taskGroups = taskGroups.Select(x => new AchievementTaskGroup(x)).ToArray();

        return clone;
    }

    // 세이브 관련 파트
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
