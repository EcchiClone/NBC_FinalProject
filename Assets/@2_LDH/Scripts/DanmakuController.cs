using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static PhaseSO;

public class DanmakuController : PoolAble
{
    public IObjectPool<GameObject> Pool { get; set; }

    private DanmakuParameters _currentParameters; // 현재 탄막의 파라미터

    // 탄막에 파라미터를 설정하는 메서드 추가
    public void Initialize(DanmakuParameters parameters, float cycleTime, List<PatternHierarchy> subPatterns)
    {
        _currentParameters = parameters;

        // 이동 및 반환 로직
        StartCoroutine(UpdateMoveParameter());
        StartCoroutine(Co_Release());
        // 하위 패턴 실행 로직
        if (subPatterns != null)
        {
            foreach (var patternHierarchy in subPatterns)
            {
                DanmakuGenerator.instance.StartPatternHierarchy(patternHierarchy, cycleTime, gameObject);
            }
        }
    }


    void Update()
    {
        Move();
    }

    void Move()
    {
        // 탄막 이동 로직
        switch (_currentParameters.danmakuMoveType)
        {
            case DanmakuMoveType.Head:
                // 오브젝트가 향하는 방향으로 이동
                transform.Translate(transform.forward * _currentParameters.speed * Time.deltaTime, Space.World);
                break;
                // 다른 이동 타입 처리
        }
    }

    IEnumerator UpdateMoveParameter()
    {
        // 이동 파라미터 업데이트, 예를 들어 방향 변경, 속도 변경 등
        // 현재 예제에서는 추가적인 이동 파라미터 업데이트가 없으므로 바로 종료
        yield return null;
    }

    IEnumerator Co_Release()
    {
        // 지정된 시간 후에 탄막을 객체 풀로 반환
        yield return new WaitForSeconds(_currentParameters.releaseTimer);
        ReleaseObject();
    }

}
