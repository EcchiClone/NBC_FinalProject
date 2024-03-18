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
        UpdateMoveParameter();
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
        Accel();
    }

    void Move()
    {
        // 탄막 이동 로직
        switch (_currentParameters.enemyBulletMoveType)
        {
            case EnemyBulletMoveType.Forward: // 오브젝트가 향하는 방향으로 이동
                transform.Translate(transform.forward * _currentParameters.speed * Time.deltaTime, Space.World);
                break;
            //case EnemyBulletMoveType.LerpToPlayer:
            //    GameObject player = GameObject.FindGameObjectWithTag("Player"); // 플레이어 찾기
            //    if (player != null)
            //    {
            //        // 플레이어를 향한 방향 벡터 계산
            //        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

            //        // 현재 방향에서 플레이어를 향한 방향으로의 회전 Quaternion 생성
            //        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            //        // 현재 회전에서 목표 회전으로 부드럽게 변환
            //        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _currentParameters.갯 * Time.deltaTime);
            //    }
            //    transform.Translate(transform.forward * _currentParameters.speed * Time.deltaTime, Space.World);
            //    break;
        }
    }
    void Accel()
    {
        //_currentParameters.speed *= _currentParameters.accelMultiple;
        //_currentParameters.speed += _currentParameters.accelPlus;
    }

    void UpdateMoveParameter()
    {
        foreach (EnemyBulletChangePropertys e in _currentParameters.enemyBulletChangeMoveProperty)
        {
            StartCoroutine(Co_UpdateMoveParameter(e));
        }
    }
    IEnumerator Co_UpdateMoveParameter(EnemyBulletChangePropertys e)
    {
        yield return new WaitForSeconds(e._timer);
        //try
        //{
        //    switch(e.)
        //    }
        //    catch {}
    }
}
