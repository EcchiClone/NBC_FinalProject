using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Phases", menuName = "DanmakuSO/Phase", order = 1)]
public class PhaseSO : ScriptableObject
{
    [System.Serializable]
    public class PatternHierarchy
    {
        [Header("About This Pattern")]
        public PatternSO patternSO;                         // 참조할 PatternSO
        public string patternName;                          // 실행할 패턴의 이름. PatternSO 에서 
        public GenCondition genCondition;                   // 해당 패턴의 생성 조건. 테스트를 위해 일단 Timer만을 구현
        public float startTime;                             // > Timer의 경우, 사이클 중 패턴이 시작할 시간
                                                            // > WithRelease, MasterTrigger 의 경우도 필요하다면 작성

        [Header("About Next Pattern")]
        public float cycleTime;                             // 하위 패턴들을 굴릴 사이클의 전체 시간
        public List<PatternHierarchy> subPatterns;          // 하위 패턴 목록
    }

    [Header("About This Phase")]
    public GenCondition genCondition;                       // 해당 페이즈 시작 조건. 테스트를 위해 일단 Timer만을 구현
    public float startTime;                                 // > Timer의 경우, 사이클 중 패턴이 시작할 시간

    [Header("About Next Pattern")]
    public float cycleTime;                                 // 하위 패턴들을 굴릴 사이클의 전체 시간
    public List<PatternHierarchy> hierarchicalPatterns;     // 계층 구조

    // Todo.
    // 여기 구조 다시 한 번 생각 해 봐야함. PatternSO를 참조하기보다 긁어와서 커스텀 할 수 있어야 할 것. // 일단 후순위로 두기.
    // PatternSO는 프리셋 모음이라는 것을 다시 한 번 상기하기
}
public enum GenCondition
{
    Timer,              // 일정시간 후 생성
    WithRelease,        // 반환과 함께 생성(불필요 예정)
    MasterTrigger,      // Manager 또는 Enemy에서 관리. 트리거 작동 시, 구독한 탄막들 일괄 터뜨리기.
}