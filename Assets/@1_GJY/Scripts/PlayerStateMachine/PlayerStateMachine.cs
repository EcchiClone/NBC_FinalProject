using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerStateMachine : MonoBehaviour
{
    // # Parts
    public LowerPart CurrentLowerPart { get; private set; }
    public UpperPart CurrentUpperPart { get; private set; }

    // # AnimationData
    [field: Header("# Animation")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }
    [SerializeField] private AnimationCurve _boostDragCurve;

    // # Class
    [field: Header("# LockOn")]
    [field: SerializeField] public LockOnSystem LockOnSystem { get; private set; }
    public PlayerStatus Player { get; private set; }

    // # Components    
    public PlayerInput P_Input { get; private set; }
    public CharacterController Controller { get; private set; }    
    public Animator Anim { get; private set; }
    public Module Module { get; private set; }
    
    // # Info
    public float InitialGravity { get; private set; }

    public Vector2 _currentMovementInput;
    public Vector3 _currentMovementDirection;
    public Vector3 _cameraRelativeMovement;

    // # States
    public bool IsMoveInputPressed { get; private set; }
    public bool IsJumpInputPressed { get; private set; }
    public bool IsDashInputPressed { get; private set; }
    public bool IsPrimaryWeaponInputPressed { get; private set; }
    public bool IsSecondaryWeaponInputPressed { get; private set; }
    public bool IsLockOn { get; private set; }
    public bool IsJumping { get; set; }
    public bool IsDashing { get; set; }
    public bool CanDash { get; set; }
    public bool CanJudgeDashing { get; set; }

    public PlayerBaseState CurrentState { get; set; }
    public PlayerStateFactory StateFactory { get; private set; }
    public WeaponTiltController TiltController { get; private set; }

    public readonly float TIME_TO_NON_COMBAT_MODE = 5f;
    public readonly float TIME_TO_SWITCHABLE_DASH_MODE = 5f;
    public readonly float MIN_GRAVITY_VALUE = -0.5f; // 접지 중일 때 최소 중력배율

    private float _dashCoolDownTime = float.MaxValue;
    private float _movementModifier = 1;
    

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // 애니메이션 Hash 초기화
        AnimationData.Init();

        // 컴포넌트 Get        
        P_Input = GetComponent<PlayerInput>();
        Controller = GetComponent<CharacterController>();
        Module = GetComponent<Module>();

        // Setup
        Managers.Module.CreateModule(Module.LowerPivot, Module);
        CurrentLowerPart = Managers.Module.CurrentLowerPart;        
        CurrentUpperPart = Managers.Module.CurrentUpperPart;        
        LockOnSystem.Setup(this);

        Anim = GetComponentInChildren<Animator>();

        Player = new PlayerStatus(CurrentLowerPart.lowerSO, CurrentUpperPart.upperSO);        
        TiltController = new WeaponTiltController(this);        
        StateFactory = new PlayerStateFactory(this);
        CurrentState = StateFactory.NonCombat();
        CurrentState.EnterState();

        // 초기값 설정
        InitialGravity = Physics.gravity.y;
        _currentMovementDirection.y = MIN_GRAVITY_VALUE;
        CanDash = true;
    }

    private void Start()
    {
        // 콜백 함수 등록
        AddInputCallBacks();
    }

    private void Update()
    {
        CurrentState.UpdateStates();

        HandleMove();
        HandleRotation();
        HandleUseWeaponPrimary();
        HandleDashCoolDown();
    }

    private void AddInputCallBacks()
    {
        P_Input.Actions.Move.started += OnMovementInput;
        P_Input.Actions.Move.performed += OnMovementInput;
        P_Input.Actions.Move.canceled += OnMovementInput;
        P_Input.Actions.Jump.started += OnJump;
        P_Input.Actions.Jump.canceled += OnJump;
        P_Input.Actions.Dash.started += OnDash;
        P_Input.Actions.Dash.canceled += OnDash;

        P_Input.Actions.PrimaryWeapon.started += OnPrimaryWeapon;
        P_Input.Actions.PrimaryWeapon.canceled += OnPrimaryWeapon;
        P_Input.Actions.SecondaryWeapon.started += OnSecondaryWeapon;
        P_Input.Actions.SecondaryWeapon.canceled += OnSecondaryWeapon;
        P_Input.Actions.LockOn.started += OnLockOn;
    }

    // InputAction에 콜백 함수로 등록하여 입력값 받아옴. (이동관련)
    private void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovementDirection.x = _currentMovementInput.x;
        _currentMovementDirection.z = _currentMovementInput.y;

        Anim.SetFloat(AnimationData.WalkFnAParameterHash, _currentMovementDirection.z);
        Anim.SetFloat(AnimationData.WalkLnRParameterHash, _currentMovementDirection.x);

        IsMoveInputPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    // InputAction에 콜백 함수로 등록하여 입력값 받아옴. (점프관련)
    private void OnJump(InputAction.CallbackContext context)
    {
        IsJumpInputPressed = context.ReadValueAsButton();
    }

    // InputAction에 콜백 함수로 등록하여 입력값 받아옴. (대쉬관련)
    private void OnDash(InputAction.CallbackContext context)
    {
        IsDashInputPressed = context.ReadValueAsButton();
    }

    // InputAction에 콜백 함수로 등록하여 입력값 받아옴. (전투관련)
    private void OnPrimaryWeapon(InputAction.CallbackContext context)
    {
        IsPrimaryWeaponInputPressed = context.ReadValueAsButton();
    }

    private void OnSecondaryWeapon(InputAction.CallbackContext context)
    {
        IsSecondaryWeaponInputPressed = context.ReadValueAsButton();
        if (IsSecondaryWeaponInputPressed)
            CurrentUpperPart.UseWeapon_Secondary();
    }

    private void OnLockOn(InputAction.CallbackContext context)
    {
        if (IsLockOn)
        {
            IsLockOn = false;
            LockOnSystem.ReleaseTarget();
        }
        else
        {
            if (LockOnSystem.IsThereEnemyScanned())
            {
                IsLockOn = true;
                LockOnSystem.LockOnTarget();
            }
        }
    }

    private void HandleMove()
    {
        _cameraRelativeMovement = ConvertToCameraSpace(_currentMovementDirection * Player.MovementSpeed * _movementModifier);
        Controller.Move(_cameraRelativeMovement * Time.deltaTime);
    }

    private void HandleUseWeaponPrimary()
    {
        if (IsPrimaryWeaponInputPressed)
            CurrentUpperPart.UseWeapon_Primary();
    }

    // 회전 제어
    private void HandleRotation()
    {
        if (CurrentState.StateType == RootStateType.Combat)
            CombatRotation();
        else
            NonCombatRotation();
    }

    #region NonCombatMode
    private void NonCombatRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = _cameraRelativeMovement.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = _cameraRelativeMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (IsMoveInputPressed && positionToLookAt != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Player.SmoothRotateValue * Time.deltaTime);
        }
    }

    // 카메라 보는 방향 기준으로 이동
    private Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {
        float currentYValue = vectorToRotate.y;

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 cameraForwardZ = vectorToRotate.z * cameraForward;
        Vector3 cameraRightX = vectorToRotate.x * cameraRight;

        Vector3 vectorRotateToCameraSpace = cameraForwardZ + cameraRightX;
        vectorRotateToCameraSpace.y = currentYValue;
        return vectorRotateToCameraSpace;
    }

    public void ResetWeaponTilt()
    {
        StartCoroutine(TiltController.CoResetRoutine());
    }

    public void StopResetWeaponTilt()
    {
        StopCoroutine(TiltController.CoResetRoutine());
    }
    #endregion

    #region CombatMode
    private void CombatRotation() // 컴뱃 + 락온일 때 틸트 조절 << 작업해야함
    {
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation;
        Vector3 positionToLookAt;

        if (IsLockOn)
        {
            TiltController.CombatLockOnControl();
            positionToLookAt = LockOnSystem.TargetEnemy.transform.position - transform.position;
        }
        else
        {
            TiltController.CombatFreeFireControl();
            positionToLookAt = Camera.main.transform.forward;
        }

        positionToLookAt.y = 0;
        targetRotation = Quaternion.LookRotation(positionToLookAt);

        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Player.SmoothRotateValue * Time.deltaTime);
    }
    #endregion

    #region Dash
    private void HandleDashCoolDown()
    {
        if (_dashCoolDownTime >= TIME_TO_SWITCHABLE_DASH_MODE)
            return;

        _dashCoolDownTime += Time.deltaTime;
        if (_dashCoolDownTime >= TIME_TO_SWITCHABLE_DASH_MODE)
        {
            CanDash = true;
            _dashCoolDownTime = TIME_TO_SWITCHABLE_DASH_MODE;
            Debug.Log("대시 가능");
        }
    }

    public void StartDash()
    {
        _dashCoolDownTime = 0;
        CanDash = false;
        CanJudgeDashing = false;
        IsDashing = true;

        CurrentLowerPart.BoostOnOff(true);
        CurrentUpperPart.BoostOnOff(true);
        StartCoroutine(CoBoostOn());
    }

    public void StopDash()
    {
        _movementModifier = 1f;
        CurrentLowerPart.BoostOnOff(false);
        CurrentUpperPart.BoostOnOff(false);
    }

    private IEnumerator CoBoostOn()
    {
        // Test 하드코딩        
        float startSpeed = _movementModifier * Player.BoostPower;
        float endSpeed = (_movementModifier + _movementModifier * startSpeed) * 0.5f;

        float current = 0f;
        float percent = 0f;
        float time = 2f;

        while (percent < 1f)
        {
            current += Time.deltaTime;
            percent = current / time;

            _movementModifier = Mathf.Lerp(startSpeed, endSpeed, _boostDragCurve.Evaluate(percent));
            yield return null;
        }
        _movementModifier = endSpeed;
        CanJudgeDashing = true;
    }
    #endregion
}