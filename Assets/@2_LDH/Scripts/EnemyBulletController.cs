using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static PhaseSO;

public class EnemyBulletController : EnemyBullet
{
    [SerializeField] TrailRenderer[] _trailRenderers;
    private EnemyBulletParameters _currentParameters; // 현재 탄막의 파라미터
    private Coroutine _releaseCoroutine;
    private GameObject _rootGo;
    private Transform _masterTf;
    private Rigidbody _rb; 


    // 탄막에 파라미터를 설정하는 메서드 추가
    public void Initialize(EnemyBulletParameters parameters, float cycleTime, List<PatternHierarchy> subPatterns, GameObject rootGo, Transform masterTf)
    {
        foreach(TrailRenderer t in _trailRenderers)
        {
            t.Clear();
        }

        _currentParameters = parameters;

        _rootGo = rootGo;

        _masterTf = masterTf;

        _rb = GetComponent<Rigidbody>();

        // 이동 및 반환 로직
        UpdateMoveParameter();
        ReleaseObject(_currentParameters.releaseTimer);

        // 하위 패턴 실행 로직
        if (subPatterns != null)
        {
            foreach (var patternHierarchy in subPatterns)
            {
                EnemyBulletGenerator.instance.StartPatternHierarchy(patternHierarchy, cycleTime, rootGo, gameObject);
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
        #region velocity 사용
        switch (_currentParameters.enemyBulletMoveType)
        {
            case EnemyBulletMoveType.Forward: // 오브젝트가 향하는 방향으로 velocity 설정
                _rb.velocity = transform.forward * _currentParameters.speed;
                break;

            case EnemyBulletMoveType.LerpToPlayer:
                GameObject player = GameObject.FindGameObjectWithTag("Player"); // 플레이어 찾기
                if (player != null)
                {
                    Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
                    Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _currentParameters.rotationSpeed * Time.deltaTime);
                    _rb.velocity = transform.forward * _currentParameters.speed;
                }
                break;
        }
        // LocalYRotation
        _rb.angularVelocity = Vector3.up * _currentParameters.localYRotationSpeed * Mathf.Deg2Rad * 360;
        #endregion

        #region Transform 사용
        // 탄막 이동 로직
        //switch (_currentParameters.enemyBulletMoveType)
        //{
        //    case EnemyBulletMoveType.Forward: // 오브젝트가 향하는 방향으로 이동
        //        transform.Translate(transform.forward * _currentParameters.speed * Time.deltaTime, Space.World);
        //        break;

        //    case EnemyBulletMoveType.LerpToPlayer:
        //        GameObject player = GameObject.FindGameObjectWithTag("Player"); // 플레이어 찾기
        //        if (player != null)
        //        {
        //            // 플레이어를 향한 방향 벡터 계산
        //            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        //            // 현재 방향에서 플레이어를 향한 방향으로의 회전 Quaternion 생성
        //            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        //            // 현재 회전에서 목표 회전으로 부드럽게 변환
        //            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _currentParameters.rotationSpeed * Time.deltaTime);
        //        }
        //        transform.Translate(transform.forward * _currentParameters.speed * Time.deltaTime, Space.World);
        //        break;
        //}
        //// LocalYRotation
        //transform.Rotate(Vector3.forward, _currentParameters.localYRotationSpeed * 360 * Time.deltaTime, Space.Self);
        #endregion
    }
    void Accel()
    {
        // 곱셈 가속
        float accelFactor = Mathf.Exp(Mathf.Log(_currentParameters.accelMultiple) * Time.deltaTime);
        _currentParameters.speed *= accelFactor;

        // 합 가속
        _currentParameters.speed += _currentParameters.accelPlus * Time.deltaTime;

        _currentParameters.speed = Mathf.Clamp(_currentParameters.speed, _currentParameters.minSpeed, _currentParameters.maxSpeed);

    }

    void UpdateMoveParameter()
    {
        foreach (EnemyBulletChangePropertys e in _currentParameters.enemyBulletChangeMoveProperty)
        {
            StartCoroutine(Co_UpdateMoveParameter(e));
        }
    }
    IEnumerator Co_UpdateMoveParameter(EnemyBulletChangePropertys e) // Move 변동 설정 각각에 대해 코루틴
    {
        yield return new WaitForSeconds(e._timer);

        if (gameObject.activeSelf == true)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            float speed = _currentParameters.speed;
            float accelMultiple = _currentParameters.accelMultiple;
            float accelPlus = _currentParameters.accelPlus;

            Vector3 moveDirection = _currentParameters.moveDirection;

            switch (e._changeSpeedType)
            {
                case EnemyBulletChangeSpeedType.Change:
                    speed = e._speed;
                    accelPlus = e._accelPlus;
                    accelMultiple = e._accelMultiple;
                    break;
                case EnemyBulletChangeSpeedType.ChangeSpeedOnly:
                    speed = e._speed;
                    break;
                case EnemyBulletChangeSpeedType.ChangeAccelOnly:
                    accelPlus = e._accelPlus;
                    accelMultiple = e._accelMultiple;
                    break;

            }

            switch (e._changeRotationType)
            {
                case EnemyBulletChangeRotationType.LookToPlayer:
                    if (player != null)
                    {
                        Vector3 directionToPlayer = player.transform.position - transform.position;
                        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                        transform.rotation = lookRotation;
                    }
                    break;
                case EnemyBulletChangeRotationType.MasterLookPlayer:
                    if (player != null && _masterTf != null)
                    {
                        Vector3 directionToPlayer = player.transform.position - _masterTf.position;
                        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                        transform.rotation = lookRotation;
                    }
                    break;
                case EnemyBulletChangeRotationType.RootLookPlayer:
                    if (player != null && _rootGo != null)
                    {
                        Vector3 directionToPlayer = player.transform.position - _rootGo.transform.position;
                        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                        transform.rotation = lookRotation;
                    }
                    break;
                case EnemyBulletChangeRotationType.LookToMaster:
                    if (_masterTf != null)
                    {
                        Vector3 directionToMaster = _masterTf.position - transform.position;
                        Quaternion lookRotation = Quaternion.LookRotation(directionToMaster);
                        transform.rotation = lookRotation;
                    }
                    break;
                case EnemyBulletChangeRotationType.LookToRoot:
                    if (_masterTf != null)
                    {
                        Vector3 directionToRoot = _rootGo.transform.position - transform.position;
                        Quaternion lookRotation = Quaternion.LookRotation(directionToRoot);
                        transform.rotation = lookRotation;
                    }
                    break;
                case EnemyBulletChangeRotationType.Reverse:
                    transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
                    break;
                case EnemyBulletChangeRotationType.World:
                    transform.rotation = Quaternion.LookRotation(e._moveDirection);
                    break;
                case EnemyBulletChangeRotationType.Local: // 회전 방식에 따라 경우가 갈림. 보류.
                    transform.rotation = Quaternion.LookRotation(e._moveDirection);
                    break;
            }

            _currentParameters = new EnemyBulletParameters(
                speed,
                _currentParameters.minSpeed,
                _currentParameters.maxSpeed,
                accelMultiple,
                accelPlus,
                e._rotationSpeed,
                _currentParameters.localYRotationSpeed,
                moveDirection,
                e._resetMoveType,
                _currentParameters.enemyBulletChangeMoveProperty,
                _currentParameters.releaseMethod,
                _currentParameters.releaseTimer
                );
        }
        try
        {
            
        } catch { }
    }
}
