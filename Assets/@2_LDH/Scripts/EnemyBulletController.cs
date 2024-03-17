using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static PhaseSO;

public class EnemyBulletController : PoolAble
{
    [SerializeField] TrailRenderer _trailRenderer;
    private EnemyBulletParameters _currentParameters; // 현재 탄막의 파라미터
    private Coroutine releaseCoroutine;

    // 탄막에 파라미터를 설정하는 메서드 추가
    public void Initialize(EnemyBulletParameters parameters, float cycleTime, List<PatternHierarchy> subPatterns)
    {
        _trailRenderer.Clear();

        _currentParameters = parameters;

        // 이동 및 반환 로직
        StartCoroutine(UpdateMoveParameter());
        ReleaseObject(_currentParameters.releaseTimer);

        // 하위 패턴 실행 로직
        if (subPatterns != null)
        {
            foreach (var patternHierarchy in subPatterns)
            {
                EnemyBulletGenerator.instance.StartPatternHierarchy(patternHierarchy, cycleTime, gameObject);
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
        switch (_currentParameters.enemyBulletMoveType)
        {
            case EnemyBulletMoveType.Head: // 오브젝트가 향하는 방향으로 이동
                transform.Translate(transform.forward * _currentParameters.speed * Time.deltaTime, Space.World);
                break;
        }
    }

    IEnumerator UpdateMoveParameter()
    {
        yield return null;
    }
}
