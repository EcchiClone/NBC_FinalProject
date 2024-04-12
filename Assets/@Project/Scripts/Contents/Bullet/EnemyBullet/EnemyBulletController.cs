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
    private Vector3 fixedPlayerPos;
    private Vector3 playerPos;
    private float _normalizedValue;

    // private float levelCoefficient; // 레벨에 따른 속도 계수. 1.0 이상.

    public void Initialize(EnemyBulletSettings settings, float cycleTime, List<PatternHierarchy> subPatterns, GameObject rootGo, Transform masterTf, float normalizedValue)
    {
        // Trail 궤적 초기화
        foreach(TrailRenderer t in _trailRenderers)
        {
            t.Clear();
        }

        _currentParameters = EnemyBulletParameters.FromSettings(settings);
        _rootGo = rootGo;
        _masterTf = masterTf;
        _rb = GetComponent<Rigidbody>();
        _normalizedValue = ((settings.enemyBulletShape == EnemyBulletShape.Custom || settings.enemyBulletShape == EnemyBulletShape.RandomVertex) && settings.useVelocityScalerFromMuzzleDist) ? normalizedValue : 1f;
        UpdateMoveParameter();
        ReleaseObject(_currentParameters.releaseTimer);

        // 하위 패턴 실행
        if (subPatterns != null)
        {
            foreach (var patternHierarchy in subPatterns)
            {
                var _genSettings = new BulletGenerationSettings
                {
                    muzzleTransform = null,
                    rootObject = rootGo,
                    masterObject = gameObject,
                    cycleTime = cycleTime,
                    isOneTime = false,
                    patternHierarchy = patternHierarchy
                };
                EnemyBulletGenerator.instance.StartPatternHierarchy(_genSettings);
            }
        }
    }

    void FixedUpdate()
    {
        Move();
        Accel();
    }

    void Move()
    {
        switch (_currentParameters._EnemyBulletMoveType)
        {
            case EnemyBulletMoveType.Forward: // 오브젝트가 향하는 방향으로 velocity 설정
                _rb.velocity = transform.forward * _currentParameters.Speed;
                break;

            case EnemyBulletMoveType.LerpToPlayer:
                playerPos = BulletMathUtils.GetPlayerPos(true); // 플레이어 찾기
                Quaternion targetRotation = Quaternion.LookRotation((playerPos - transform.position).normalized);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _currentParameters.RotationSpeed * Time.deltaTime);
                _rb.velocity = transform.forward * _currentParameters.Speed;
                break;
        }
        _rb.velocity *= _normalizedValue;
        // LocalYRotation
        float speedInRadiansPerSecond = _currentParameters.LocalYRotationSpeed * 2 * Mathf.PI; // 바퀴 수를 라디안/초로 변환
        float rotationThisFrame = speedInRadiansPerSecond * Time.deltaTime; // 현재 프레임에서의 회전량 계산
        Quaternion localRotation = Quaternion.Euler(0, 0, rotationThisFrame * Mathf.Rad2Deg); // Quaternion.Euler는 도(degree) 단위를 사용
        _rb.rotation = _rb.rotation * localRotation;
    }

    void Accel()
    {
        // 가속(곱연산)
        float accelFactor = Mathf.Exp(Mathf.Log(_currentParameters.AccelMultiple) * Time.deltaTime);
        _currentParameters.Speed *= accelFactor;
        // 가속(합연산)
        _currentParameters.Speed += _currentParameters.AccelPlus * Time.deltaTime;
        // Clamp
        _currentParameters.Speed = Mathf.Clamp(_currentParameters.Speed, _currentParameters.MinSpeed, _currentParameters.MaxSpeed);
    }

    void UpdateMoveParameter()
    {
        foreach (EnemyBulletChangePropertys e in _currentParameters._EnemyBulletChangeMoveProperty)
        {
            StartCoroutine(Co_UpdateMoveParameter(e));
        }
    }

    IEnumerator Co_UpdateMoveParameter(EnemyBulletChangePropertys e) // Move 변동 설정 각각에 대해 코루틴
    {
        //yield return new WaitForSeconds(e._timer);
        yield return Util.GetWaitSeconds(e._timer);

        if (gameObject.activeSelf == true)
        {
            fixedPlayerPos = BulletMathUtils.GetPlayerPos();
            playerPos = fixedPlayerPos;

            float speed = _currentParameters.Speed;
            float accelMultiple = _currentParameters.AccelMultiple;
            float accelPlus = _currentParameters.AccelPlus;

            Vector3 moveDirection = _currentParameters.MoveDirection;
            Vector3 moveDirectionAim = _currentParameters.MoveDirection;

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
                    transform.rotation = Quaternion.LookRotation(playerPos - transform.position);
                    break;
                case EnemyBulletChangeRotationType.MasterLookPlayer:
                    if (_masterTf != null)
                        transform.rotation = Quaternion.LookRotation(playerPos - _masterTf.position);
                    break;
                case EnemyBulletChangeRotationType.RootLookPlayer:
                    if (_rootGo != null)
                        transform.rotation = Quaternion.LookRotation(playerPos - _rootGo.transform.position);
                    break;
                case EnemyBulletChangeRotationType.LookToMaster:
                    if (_masterTf != null)
                        transform.rotation = Quaternion.LookRotation(_masterTf.position - transform.position);
                    break;
                case EnemyBulletChangeRotationType.LookToRoot:
                    if (_masterTf != null)
                        transform.rotation = Quaternion.LookRotation(_rootGo.transform.position - transform.position);
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
                case EnemyBulletChangeRotationType.CompletelyRandom:
                    transform.rotation = Random.rotation;
                    break;
            }

            // new 클래스 없도록 작성
            this._currentParameters.Speed = speed;
            //_currentParameters.MinSpeed;
            //_currentParameters.MaxSpeed;
            this._currentParameters.AccelMultiple = accelMultiple;
            this._currentParameters.AccelPlus = accelPlus;
            this._currentParameters.RotationSpeed = e._rotationSpeed;
            //this._currentParameters.LocalYRotationSpeed
            this._currentParameters.MoveDirection = moveDirection;
            this._currentParameters.MoveDirectionAim = moveDirectionAim;
            this._currentParameters._EnemyBulletMoveType = e._resetMoveType;
            //this._currentParameters._EnemyBulletChangeMoveProperty
            //this._currentParameters._ReleaseMethod;
            //this._currentParameters.ReleaseTimer;
        }
    }

    private void ReleaseObject(float releaseTimer)
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine("Co_DisableWithTimer", releaseTimer);
        else
            Debug.LogWarning("해당 오브젝트는 이미 inactive 상태: " + gameObject.name);
    }
    private IEnumerator Co_DisableWithTimer(float releaseTimer)
    {
        yield return Util.GetWaitSeconds(releaseTimer);
        gameObject.SetActive(false);
    }
}
