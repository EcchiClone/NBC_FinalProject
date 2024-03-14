using UnityEngine;
using System.Collections;

public class EnemyDanmakuTrigger : MonoBehaviour
{
    public PhaseSO currentPhase;

    public bool isShooting = true;  // false가 되면 DanmakuGenerator에서 생성 작업을 멈춘다.
                                    // 사이클 돌리던 코루틴이 완전히 멈추는 것이므로 StartPatternHierarchy 를 다시 해줘야 함.
    public bool[] isTriggerOn;      // 탄막 일괄 이벤트를 위한 트리거를 위해 추후 준비.

    private void Start()
    {
        foreach (var patternHierarchy in currentPhase.hierarchicalPatterns)
        {
            DanmakuGenerator.instance.StartPatternHierarchy(patternHierarchy, currentPhase.cycleTime, gameObject);
        }
    }
}