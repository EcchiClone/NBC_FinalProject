using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    // # AnimationData
    [field: Header("# Animation")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }
    [SerializeField] private AnimationCurve _boostDragCurve;

    // # Components    
    public PlayerInput P_Input { get; private set; }
    public CharacterController Controller { get; private set; }
    public Animator Anim { get; private set; }
    public Module Module { get; private set; }
    public Player_Sound Sound { get; private set; }

    // # Info
    public float InitialGravity { get; private set; }

    public Vector2 _currentMovementInput;
    public Vector3 _currentMovementDirection;
    public Vector3 _cameraRelativeMovement;

    // # States
    [field: SerializeField] public bool IsDead { get; private set; } = false;
    [field: SerializeField] public bool IsMoveInputPressed { get; private set; }
    [field: SerializeField] public bool IsJumpInputPressed { get; private set; }
    [field: SerializeField] public bool IsDashInputPressed { get; private set; }
    [field: SerializeField] public bool IsLeftArmWeaponInputPressed { get; private set; }
    [field: SerializeField] public bool IsRightArmWeaponInputPressed { get; private set; }
    [field: SerializeField] public bool IsLeftShoulderWeaponInputPressed { get; private set; }
    [field: SerializeField] public bool IsRightShoulderWeaponInputPressed { get; private set; }
    [field: SerializeField] public bool IsJumping { get; set; }
    [field: SerializeField] public bool IsRun { get; set; }
    [field: SerializeField] public bool IsHovering { get; set; }
    [field: SerializeField] public bool IsCanHovering { get; set; }
    [field: SerializeField] public bool CanDash { get; set; }
    [field: SerializeField] public bool CanJudgeDashing { get; set; }
    [field: SerializeField] public bool IsUsingBoost { get; set; }

    public PlayerBaseState CurrentState { get; set; }
    public PlayerStateFactory StateFactory { get; private set; }

    public readonly float TIME_TO_NON_COMBAT_MODE = 5f;    
    public readonly float GRAVITY_VALUE = 4f; // 중력배율
    public readonly float MIN_GRAVITY_VALUE = -1f; // 접지 중일 때 최소 중력배율
    public readonly float MAX_HOVER_VALUE = 5f; // 최대 호버링 상승률

    private float _movementModifier = 1f;
    private readonly float DEFAULT_SPEED_MODIFIER = 1f;    

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // 애니메이션 Hash 초기화
        AnimationData.Init();

        // 컴포넌트 Get        
        P_Input = GetComponent<PlayerInput>();
        Controller = GetComponent<CharacterController>();
        Module = GetComponent<Module>();
        Sound = GetComponent<Player_Sound>();

        // Setup
        Managers.ActionManager.OnPlayerDead += PlayerDestroyed;
        Managers.ActionManager.OnLockTargetDestroyed += CheckLockTargetIsNull;
    }

    private void Start()
    {
        PlayerSetting();        
    }

    private void PlayerSetting()
    {
        AddInputCallBacks();

        StateFactory = new PlayerStateFactory(this);

        Anim = GetComponentInChildren<Animator>();
        CanJudgeDashing = true;
        CurrentState = StateFactory.NonCombat();
        CurrentState.EnterState();

        InitialGravity = Physics.gravity.y;
        _currentMovementDirection.y = MIN_GRAVITY_VALUE;
        CanDash = true;
    }

    private void Update()
    {
        if (Managers.Scene.CurrentScene.Scenes != Define.Scenes.TutorialScene && !Managers.SpawnManager.IsStarted)
            return;
        if (Module.ModuleStatus.IsDead)
            return;

        CurrentState.UpdateStates();

        HandleMove();
        HandleRotation();
        HandleUseWeapon();
        HandleBoostRecharge();
    }

    #region InputCallBacks
    private void AddInputCallBacks()
    {
        P_Input.Actions.Move.started += OnMovementInput;
        P_Input.Actions.Move.performed += OnMovementInput;
        P_Input.Actions.Move.canceled += OnMovementInput;
        P_Input.Actions.Jump.started += OnJump;
        P_Input.Actions.Jump.canceled += OnJump;
        P_Input.Actions.Jump.canceled += OnHovering;
        P_Input.Actions.Dash.started += OnDash;
        P_Input.Actions.Dash.canceled += OnDash;
        P_Input.Actions.Dash.canceled += OnCanDash;

        P_Input.Actions.LeftArm.started += OnLeftArmWeapon;
        P_Input.Actions.LeftArm.canceled += OnLeftArmWeapon;
        P_Input.Actions.RightArm.started += OnRightArmWeapon;
        P_Input.Actions.RightArm.canceled += OnRightArmWeapon;
        P_Input.Actions.LeftShoulder.started += OnLeftShoulderWeapon;
        P_Input.Actions.LeftShoulder.canceled += OnLeftShoulderWeapon;
        P_Input.Actions.RightShoulder.started += OnRightShoulderWeapon;
        P_Input.Actions.RightShoulder.canceled += OnRightShoulderWeapon;
        P_Input.Actions.LockOn.started += OnLockOn;

        P_Input.Actions.RePair.started += OnRepair;
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
    private void OnJump(InputAction.CallbackContext context) => IsJumpInputPressed = context.ReadValueAsButton();
    private void OnHovering(InputAction.CallbackContext context) => IsCanHovering = !context.ReadValueAsButton();
    // InputAction에 콜백 함수로 등록하여 입력값 받아옴. (대쉬관련)
    private void OnDash(InputAction.CallbackContext context) => IsDashInputPressed = context.ReadValueAsButton();
    private void OnCanDash(InputAction.CallbackContext context) => CanDash = !context.ReadValueAsButton();
    // InputAction에 콜백 함수로 등록하여 입력값 받아옴. (전투관련)
    private void OnLeftArmWeapon(InputAction.CallbackContext context) => IsLeftArmWeaponInputPressed = context.ReadValueAsButton();
    private void OnRightArmWeapon(InputAction.CallbackContext context) => IsRightArmWeaponInputPressed = context.ReadValueAsButton();
    private void OnLeftShoulderWeapon(InputAction.CallbackContext context) => IsLeftShoulderWeaponInputPressed = context.ReadValueAsButton();
    private void OnRightShoulderWeapon(InputAction.CallbackContext context) => IsRightShoulderWeaponInputPressed = context.ReadValueAsButton();


    private void OnRepair(InputAction.CallbackContext context)
    {
        if (Module.Skill.IsActive)
        {
            Module.Skill.UseSkill(Module);
            StartCoroutine(Module.Skill.Co_CoolDown());
        }
    }
    #endregion

    private void PlayerDestroyed()
    {
        IsDead = true;
        Cursor.lockState = CursorLockMode.Confined;

        gameObject.SetActive(false);
    }

    private void OnLockOn(InputAction.CallbackContext context)
    {
        if (Module.LockOnSystem.IsLockon)
            Module.LockOnSystem.ReleaseTarget();
        else
        {
            if (Module.LockOnSystem.IsThereEnemyScanned())
                Module.LockOnSystem.LockOnTarget();
        }
    }

    private void CheckLockTargetIsNull(ITarget prevTarget)
    {
        if (IsDead)
            return;
        Module.LockOnSystem.LockTargetChange(prevTarget);
    }

    private void HandleMove()
    {
        Vector3 nextDir = new Vector3(
            _currentMovementDirection.x * _movementModifier * Module.ModuleStatus.MovementSpeed,
            _currentMovementDirection.y * GRAVITY_VALUE,
            _currentMovementDirection.z * _movementModifier * Module.ModuleStatus.MovementSpeed);

        _cameraRelativeMovement = ConvertToCameraSpace(nextDir);
        Controller.Move(_cameraRelativeMovement * Time.deltaTime);
    }

    private void HandleUseWeapon() // 대시 중 팔 무기 사용 불가
    {
        if (IsLeftShoulderWeaponInputPressed)
            Module.CurrentLeftShoulder.UseWeapon();
        if (IsRightShoulderWeaponInputPressed)
            Module.CurrentRightShoulder.UseWeapon();
        
        if (!CanJudgeDashing)
            return;

        if (IsLeftArmWeaponInputPressed)
            Module.CurrentLeftArm.UseWeapon();
        if (IsRightArmWeaponInputPressed)
            Module.CurrentRightArm.UseWeapon();
    }

    // 회전 제어
    private void HandleRotation()
    {
        if (CurrentState.StateType == RootStateType.Combat)
            CombatRotation();
        else
            NonCombatRotation();
    }

    private void HandleBoostRecharge()
    {
        if (!IsUsingBoost)
            Module.ModuleStatus.BoosterRecharge();
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
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Module.ModuleStatus.SmoothRotateValue * Time.deltaTime);
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
        StartCoroutine(Module.TiltController.CoResetRoutine());
    }

    public void StopResetWeaponTilt()
    {
        StopCoroutine(Module.TiltController.CoResetRoutine());
    }
    #endregion

    #region CombatMode
    private void CombatRotation()
    {
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation;
        Vector3 positionToLookAt;

        if (Module.LockOnSystem.IsLockon)
        {
            Module.TiltController.CombatLockOnControl();
            positionToLookAt = Module.LockOnSystem.TargetEnemy.Center - transform.position;
        }
        else
        {
            Module.TiltController.CombatFreeFireControl();
            positionToLookAt = Camera.main.transform.forward;
        }

        positionToLookAt.y = 0;
        targetRotation = Quaternion.LookRotation(positionToLookAt);

        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Module.ModuleStatus.SmoothRotateValue * Time.deltaTime);
    }
    #endregion

    #region Dash
    public void StopRun()
    {
        IsRun = false;
        _movementModifier = 1f;
        Module.CurrentLower.BoostOnOff(false);
        Module.CurrentUpper.BoostOnOff(false);
        Module.CurrentLower.FootSparksOnOff(false);
        Sound.IsDragging = false;
    }

    public IEnumerator Co_BoostOn(UnityAction changeStateAction)
    {
        Module.CurrentLower.BoostOnOff(true);
        Module.CurrentUpper.BoostOnOff(true);
        if (Controller.isGrounded)
        {
            Sound.IsDragging = true;
            Module.CurrentLower.FootSparksOnOff(true);
        }

        IsRun = true;
        CanDash = false;
        CanJudgeDashing = false;
        IsUsingBoost = true;

        float startSpeed = DEFAULT_SPEED_MODIFIER * Module.ModuleStatus.BoostPower;
        float endSpeed = (DEFAULT_SPEED_MODIFIER + DEFAULT_SPEED_MODIFIER * startSpeed) * 0.35f;

        float current = 0f;
        float percent = 0f;
        float timeToRun = Util.GetCurrentAnimationClipLength(Anim);

        while (percent < 1f)
        {
            current += Time.deltaTime;
            percent = current / timeToRun;

            if (!IsMoveInputPressed)
                break;
            _movementModifier = Mathf.Lerp(startSpeed, endSpeed, _boostDragCurve.Evaluate(percent));

            yield return null;
        }
        _movementModifier = endSpeed;
        CanJudgeDashing = true;
        IsUsingBoost = false;        

        changeStateAction.Invoke();
    }
    #endregion
}