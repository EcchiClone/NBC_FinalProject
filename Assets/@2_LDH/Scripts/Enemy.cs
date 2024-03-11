using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour // PM.07:41 적 PhaseSO 사용 테스트 중
{
    public PhaseSO currentPhase;

    private void Start()
    {
        // 페이즈 시작
        StartCoroutine(Co_ExecutePhase());
    }

    IEnumerator Co_ExecutePhase()
    {
        // 최상위 계층 사이클 시간 동안 반복 실행.
        // 적 오브젝트가 파괴되면... 멈출거라고는 일단 생각은 함.
        while (true)
        {
            foreach (var patternHierarchy in currentPhase.hierarchicalPatterns)
            {
                // 하위 패턴을 실행하는 코루틴 시작
                StartCoroutine(Co_ExecutePatternHierarchy(patternHierarchy, 0));
            }

            // 전체 사이클 시간 대기
            yield return new WaitForSeconds(currentPhase.cycleTime);
        }
    }

    IEnumerator Co_ExecutePatternHierarchy(PhaseSO.PatternHierarchy patternHierarchy, float parentStartTime)
    {
        // startTime까지 대기 후에 패턴 발현
        yield return new WaitForSeconds(patternHierarchy.startTime - parentStartTime);
        ExecutePattern(patternHierarchy.patternSO, patternHierarchy.patternName);

        // 하위 패턴이 있는 경우, 각 하위 패턴에 대해 Co_ExecutePatternHierarchy 재귀 호출. 잘 될런지는 한번 확인 해 봐야함.
        foreach (var subPattern in patternHierarchy.subPatterns)
        {
            StartCoroutine(Co_ExecutePatternHierarchy(subPattern, patternHierarchy.startTime));
        }
    }

    void ExecutePattern(PatternSO patternSO, string patternName)
    {
        // Todo:ChangeLogic
        // patternSO에서 patternName에 해당하는 패턴 데이터를 찾아, 이를 사용한 탄막 생성..... 이지만
        // 조만간 PhaseSO( List<PatternHierarchy> subPatterns )에 모든 정보를 패킹하여 자식 탄막에 전달되도록 할 것임.
        // 최적화는 이후 생각.
        var patternData = patternSO.GetSpawnInfoByPatternName(patternName);
        if (patternData != null)
        {
            Debug.Log("여기에서 Pool에서 탄막 생성 후, 만들어진 탄막에 파라미터 전달 및 DanmakuController 부착하기");
        }
    }
}
