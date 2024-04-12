using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;

public class BulletPatternLoop : MonoBehaviour
{
    // 동일 오브젝트의 EnemyPhaseStarter 컴포넌트에 적용되어 있는 Pattern목록 또는 Phase목록의 전체를 반복재생합니다.

    EnemyPhaseStarter ep;
    [SerializeField] bool isLoopPhase;
    [SerializeField] bool isLoopPattern;
    int phaseMax;
    int patternMax;
    float cycleTime;

    void Start()
    {
        ep = GetComponent<EnemyPhaseStarter>();
        phaseMax = ep.Phases.Count();
        patternMax = ep.Patterns.Count();
        cycleTime = 3f;

        StartCoroutine(Co_PhaseLoop());
        StartCoroutine(Co_PatternLoop());
    }

    private IEnumerator Co_PhaseLoop()
    {
        while (true)
        {
            while (phaseMax > 0 && isLoopPhase)
            {
                for (int i = 0; i < phaseMax && isLoopPhase; i++)
                {
                    try { ep.StartPhase(i, 0, true); Debug.Log($">>> 페이즈({ep.Phases[i].name}) 실행"); }
                    catch { Debug.LogError($"Phase[{i}] 를 실행할 수 없음."); }

                    yield return Util.GetWaitSeconds(cycleTime);
                }
            }
            yield return Util.GetWaitSeconds(0.1f);
        }
    }
    private IEnumerator Co_PatternLoop()
    {
        while (true)
        {
            while (patternMax > 0 && isLoopPattern)
            {
                for (int i = 0; i < patternMax && isLoopPattern; i++)
                {
                    try { ep.StartPattern(i, 1f, 0, true); Debug.Log($">>> 패턴({ep.Patterns[i].patternName}) 실행"); }
                    catch { Debug.LogError($"Pattern[{i}] 를 실행할 수 없음."); }
                    
                    yield return Util.GetWaitSeconds(cycleTime);
                }
            }
            yield return Util.GetWaitSeconds(0.1f);
        }
    }
}
