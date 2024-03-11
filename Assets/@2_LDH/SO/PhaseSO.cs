using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Phases", menuName = "DanmakuSO/Phase", order = 1)]
public class PhaseSO : ScriptableObject
{
    [System.Serializable]
    public class PatternHierarchy
    {
        public PatternSO patternSO;                         // 참조할 PatternSO
        public string patternName;                          // 실행할 패턴의 이름. PatternSO 에서 
        public float startTime;                             // 사이클 중 패턴이 시작할 시간
        public float cycleTime;                             // 하위 패턴들을 굴릴 사이클의 전체 시간
        public List<PatternHierarchy> subPatterns;          // 하위 패턴 목록
    }

    public float cycleTime;                                 // 하위 패턴들을 굴릴 사이클의 전체 시간
    public List<PatternHierarchy> hierarchicalPatterns;     // 계층 구조

    // Todo.
    // 여기 구조 다시 한 번 생각 해 봐야함. PatternSO를 참조하기보다 긁어와서 커스텀 할 수 있어야 할 것.
    // PatternSO는 프리셋 모음이라는 것을 다시 한 번 상기하기
}