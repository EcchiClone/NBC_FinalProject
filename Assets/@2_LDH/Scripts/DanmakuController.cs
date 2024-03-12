using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static PhaseSO;

public class DanmakuController : PoolAble
{
    private DanmakuParameters _currentParameters; // 현재 탄막의 파라미터
    private Coroutine releaseCoroutine;

    // 탄막에 파라미터를 설정하는 메서드 추가
    public void Initialize(DanmakuParameters parameters, float cycleTime, List<PatternHierarchy> subPatterns)
    {
        _currentParameters = parameters;

        // 이동 및 반환 로직
        StartCoroutine(UpdateMoveParameter());
        releaseCoroutine = StartCoroutine(Co_Release());
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
        //if (_currentParameters == null) // 왜 null이 되는지 예상이 안되는데, 무튼 자주 되어서 그를 위한 처리
        //{
        //    //StopCoroutine(releaseCoroutine); // 오류 여기
        //    ReleaseObject();
        //}
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
        yield return null;
    }

    IEnumerator Co_Release()
    {
        yield return new WaitForSeconds(_currentParameters.releaseTimer);
        ReleaseObject();
    }

    //private void OnDisable()
    //{
    //}

}
