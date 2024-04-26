using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using static PhaseSO;

public class EnemyBulletController : EnemyBullet
{
    public event EnemyBulletPatternStarter.BulletStoper onStopBullet;
    public void StopBullet()
    {
        Debug.Log("이전 패턴 멈추기");
        onStopBullet?.Invoke();
    }

    [SerializeField] TrailRenderer[] _trailRenderers;
    private EnemyBulletParameters _currentParameters; // 현재 탄막의 파라미터
    private Coroutine _releaseCoroutine;
    private GameObject _rootGo;
    private Transform _masterTf;
    private Rigidbody _rb;
    private Vector3 noiseAll;
    private Vector3 noiseSide;
    private Vector3 fixedPlayerPos;
    private Vector3 playerPos;
    private Vector3 fixedMuzzlePos;
    private float _normalizedValue;
    private bool initialized;
    // private float levelCoefficient; // 레벨에 따른 속도 계수. 1.0 이상.

    public void Initialize(EnemyBulletSettings settings, float cycleTime, List<PatternHierarchy> subPatterns, GameObject rootGo, Transform masterTf, Vector3 muzzlePos, float normalizedValue)
    {
        initialized = false;
        // Trail 궤적 초기화
        foreach (TrailRenderer t in _trailRenderers)
        {
            t.Clear();
        }
        this.noiseAll = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)) * 1f;
        this.noiseSide = Vector3.Cross((BulletMathUtils.GetPlayerPos(true) - transform.position).normalized, Vector3.up).normalized * UnityEngine.Random.Range(-1f, 1f) * 10f;

        fixedPlayerPos = BulletMathUtils.GetPlayerPos();
        fixedMuzzlePos = muzzlePos;
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
                EnemyBulletGenerator.instance.StartPatternHierarchy(_genSettings, onStopBullet);
            }
        }
    }

    void FixedUpdate()
    {
        Move();
        Accel();
    }
    private void OnEnable()
    {
        onStopBullet += EnemyBulletGenerator.instance.StopAllCoroutinesPattern;
    }
    private void OnDisable()
    {
        onStopBullet -= EnemyBulletGenerator.instance.StopAllCoroutinesPattern;
    }


    void Move()
    {
        Quaternion targetRotation;
        Vector3 centerPosition;
        Vector3 directionFromCenter;
        Vector3 horizontalDirection;
        Vector3 outwardDirection;
        float heightFactor;
        Vector3 verticalDirection;
        Vector3 tangentDirection;
        Vector3 finalVelocity;

        switch (_currentParameters._EnemyBulletMoveType)
        {
            case EnemyBulletMoveType.Forward: // 오브젝트가 향하는 방향으로 velocity 설정
                _rb.velocity = transform.forward * _currentParameters.Speed;
                break;

            case EnemyBulletMoveType.LerpToPlayer:
                playerPos = BulletMathUtils.GetPlayerPos(true); // 플레이어 찾기
                targetRotation = Quaternion.LookRotation((playerPos - transform.position).normalized);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _currentParameters.RotationSpeed * Time.deltaTime);
                _rb.velocity = transform.forward * _currentParameters.Speed;
                break;

            case EnemyBulletMoveType.LerpToPlayerNoise:
                playerPos = BulletMathUtils.GetPlayerPos(true) + this.noiseAll + this.noiseSide; // 플레이어 찾기
                targetRotation = Quaternion.LookRotation((playerPos - transform.position).normalized);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _currentParameters.RotationSpeed * Time.deltaTime);
                _rb.velocity = transform.forward * _currentParameters.Speed;
                break;

            case EnemyBulletMoveType.MasterCenter:
                centerPosition = _rootGo.transform.position; // Enemy의 현재 위치
                directionFromCenter = (transform.position - centerPosition).normalized; // 중심에서 탄환으로의 방향 벡터

                // 수평 방향 벡터 계산 (Y축 변화 포함하지 않음)
                horizontalDirection = new Vector3(directionFromCenter.x, 0, directionFromCenter.z).normalized;

                // 바깥쪽으로 나아가는 방향을 수평 평면에서 계산, 각도를 조정하여 바깥으로 확장
                outwardDirection = Quaternion.Euler(0, 45, 0) * horizontalDirection;

                // Y축 변화를 포함하는 벡터 추가
                heightFactor = 1.0f; // Y축 변화량을 조절하는 계수, 필요에 따라 조정
                verticalDirection = new Vector3(0, directionFromCenter.y * heightFactor, 0);

                // 원의 접선 방향을 수평 평면에서 계산
                tangentDirection = Vector3.Cross(Vector3.up, horizontalDirection).normalized;

                // 최종 벡터는 탄점 방향과 바깥쪽으로 나아가는 벡터 및 수직 방향 벡터의 합
                finalVelocity = (tangentDirection + outwardDirection + verticalDirection).normalized * _currentParameters.Speed;

                targetRotation = Quaternion.LookRotation(finalVelocity); // 탄환의 회전을 최종 벡터 방향으로 설정
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _currentParameters.RotationSpeed * Time.deltaTime);

                _rb.velocity = finalVelocity; // 탄환의 속도를 최종 계산된 벡터로 설정
                break;

            case EnemyBulletMoveType.MuzzleCenter:
                centerPosition = fixedMuzzlePos; // Enemy의 현재 위치
                directionFromCenter = (transform.position - centerPosition).normalized; // 중심에서 탄환으로의 방향 벡터

                // 수평 방향 벡터 계산 (Y축 변화 포함하지 않음)
                horizontalDirection = new Vector3(directionFromCenter.x, 0, directionFromCenter.z).normalized;

                // 바깥쪽으로 나아가는 방향을 수평 평면에서 계산, 각도를 조정하여 바깥으로 확장
                outwardDirection = Quaternion.Euler(0, 45, 0) * horizontalDirection;

                // Y축 변화를 포함하는 벡터 추가
                heightFactor = 1.0f; // Y축 변화량을 조절하는 계수, 필요에 따라 조정
                verticalDirection = new Vector3(0, directionFromCenter.y * heightFactor, 0);

                // 원의 접선 방향을 수평 평면에서 계산
                tangentDirection = Vector3.Cross(Vector3.up, horizontalDirection).normalized;

                // 최종 벡터는 탄점 방향과 바깥쪽으로 나아가는 벡터 및 수직 방향 벡터의 합
                finalVelocity = (tangentDirection + outwardDirection + verticalDirection).normalized * _currentParameters.Speed;

                targetRotation = Quaternion.LookRotation(finalVelocity); // 탄환의 회전을 최종 벡터 방향으로 설정
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _currentParameters.RotationSpeed * Time.deltaTime);

                _rb.velocity = finalVelocity; // 탄환의 속도를 최종 계산된 벡터로 설정
                break;

            case EnemyBulletMoveType.Nature:
                if (!initialized)
                {
                    // 초기 방향과 속도 설정
                    _rb.velocity = transform.forward * _currentParameters.Speed; // 예시로 transform.forward를 사용
                    initialized = true; // 초기화 완료 표시
                }
                // 이후에는 중력 및 Unity 물리 엔진에 의해 자동으로 처리
                break;


        }
        _rb.velocity *= _normalizedValue;

        // 중력 가속도 적용: _currentParameters.Gravity 값을 사용하여 -Y 방향으로 가속
        if (_currentParameters.Gravity != 0)
        {
            _rb.velocity += Vector3.down * _currentParameters.Gravity * Time.deltaTime;
        }

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

        // _currentParameters.Gravity 수치로 -Y방향의 속력에 적용되도록.
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
                    transform.rotation = UnityEngine.Random.rotation; 
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
