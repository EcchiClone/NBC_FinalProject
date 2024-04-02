using System.Linq;
using UnityEngine;

public enum TaskState
{
    Inactive,
    Running,
    Complete,
}

[CreateAssetMenu(menuName = "Achievement/Task/Task", fileName = "Task_")]
public class AchievementTask : ScriptableObject
{
    #region Events

    public delegate void StateChangedHandler(AchievementTask task, TaskState currentState, TaskState prevState);
    // CurrentSuccess 값이 변했을 때 알려주는 event. State와 마찬가지로 다른 곳에서 계속 Update로 추적하지 않아도 되게 하기 위함.
    public delegate void SuccessChangedHandler(AchievementTask task, int currentSuccess, int prevSuccess);

    #endregion

    [SerializeField]
    private TaskCategory category;

    [Header("Text")]
    [SerializeField]
    private string codeName; // Task 구분을 위한 Key로써 추후 사용 가능
    [SerializeField]
    private string description;

    [Header("Action")]
    [SerializeField]
    private TaskAction action; // 세는 방식

    [Header("Target")]
    [SerializeField]
    private TaskTarget[] targets; // 타겟이 여러 종류일 가능성을 위해 배열로 관리

    [Header("Settings")]
    [SerializeField]
    private InitialSuccessValue initialSuccessValue; // 퀘 시작 시점의 달성도 수치
    [SerializeField]
    private int needSuccessToComplete; // 달성에 필요한 수치
    [SerializeField]
    private bool canReceiveReportsDuringCompletion; // Task가 완료되었어도 계속 성공횟수를 보고받을 것인지에 대한 옵션.
    // 예를들어 Item 100개를 모아 완료하는 Quest인데, User가 아이템을 100개를 모았지만 Quest를 완료하기 전에 50개를 버릴 경우, 더 이상 보고를 안 받아 버리면 Task는 여전히 완료되어있는 상태라 Quest를 완료 할 수 있음.

    private TaskState state;
    private int currentSuccess;

    public event StateChangedHandler onStateChanged; // Task 상태 설정 시(실제 코드에서는 변화가 아니라 Set 시에 호출), 사용할 메서드를 여기에 구독
    public event SuccessChangedHandler onSuccessChanged; // Task Success 시 사용할 메서드를 여기에 구독

    public int CurrentSuccess
    {
        get => currentSuccess;
        set
        {
            int prevSuccess = currentSuccess;
            currentSuccess = Mathf.Clamp(value, 0, needSuccessToComplete); // currentSuccess의 최소최대 한도 제한
            if (currentSuccess != prevSuccess)
            {
                State = currentSuccess == needSuccessToComplete ? TaskState.Complete : TaskState.Running;
                onSuccessChanged?.Invoke(this, currentSuccess, prevSuccess);
            }
        }
    }
    public TaskCategory Category => category;
    public string CodeName => codeName;
    public string Description => description;
    public int NeedSuccessToComplete => needSuccessToComplete; // 읽기 전용 프로퍼티 정의. Expression-bodied property

    public TaskState State
    {
        get => state;
        set
        {
            var prevState = state;
            state = value;
            onStateChanged?.Invoke(this, state, prevState);
        }
    }
    public bool IsComplete => State == TaskState.Complete;
    public Achievement Owner { get; private set; }

    public void Setup(Achievement owner)
    {
        Owner = owner;
    }
    public void Start()
    {
        State = TaskState.Running;
        if (initialSuccessValue)
            CurrentSuccess = initialSuccessValue.GetValue(this); // 세팅이 있을 경우에만 가져온다
    }
    public void End()
    {
        onStateChanged = null; // 이벤트 초기화
        onSuccessChanged = null; // 이벤트 초기화
    }

    public void ReceiveReport(int successCount)
    {
        CurrentSuccess = action.Run(this, CurrentSuccess, successCount); // Run은 로직을 실행한 결과 값을 반환. 첫번째 인자는 AchievementTask
    }
    public void Complete() // 강제완료
    {
        CurrentSuccess = needSuccessToComplete;
    }
    public bool IsTarget(string category, object target)
        => Category == category &&
        targets.Any(x => x.IsEqual(target)) &&
        (!IsComplete || (IsComplete && canReceiveReportsDuringCompletion));
}
