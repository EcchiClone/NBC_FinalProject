using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("# Debuging Panel")]
    public Vector3 currentCamRelative;

    [Header("# Test")]
    [Range(2f, 10f)][SerializeField] float _movementSpeed;
    [Range(5f, 15f)][SerializeField] float _smoothRotateValue;
    [field: Range(0.1f, 1f)][field: SerializeField] public float MinDownForceValue { get; private set; } // 접지 중일 때 최소 중력배율
    [field: Range(1f, 100f)][field: SerializeField] public float JumpPower { get; private set; } // 접지 중일 때 최소 중력배율

    // # Cam
    [field: SerializeField] public CinemachineFreeLook CurrentCam { get; private set; }

    // # Parts Temp
    public LowerPart CurrentLowerPart { get; private set; }
    public UpperPart CurrentUpperPart { get; private set; }

    // # AnimationData
    [field: Header("# Animation")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    // # Components
    public Module Module { get; private set; }
    public PlayerInput P_Input { get; private set; }
    public CharacterController Controller { get; private set; }
    public Animator Anim { get; private set; }

    // # Info
    public float InitialGravity { get; private set; }

    public Vector2 _currentMovementInput;
    public Vector3 _currentMovementDirection;
    public Vector3 _cameraRelativeMovement;

    // # States
    public bool IsMoveInputPressed { get; private set; }
    public bool IsJumpInputPressed { get; private set; }
    public bool IsPrimaryWeaponInputPressed { get; private set; }
    public bool IsSecondaryWeaponInputPressed { get; private set; }
    public bool IsJumping { get; set; }

    public PlayerBaseState CurrentState { get; set; }
    public PlayerStateFactory StateFactory { get; private set; }

    public readonly float TIME_TO_NON_COMBAT_MODE = 5f;

    private void Awake()
    {
        // 시네머신 카메라 초기화
        GameObject cam = GameObject.Find("@FollowCam");
        CurrentCam = cam.GetComponent<CinemachineFreeLook>();
        CurrentCam.Follow = transform;
        CurrentCam.LookAt = transform;

        // 애니메이션 Hash 초기화
        AnimationData.Init();

        // 컴포넌트 Get
        Module = GetComponent<Module>();
        P_Input = GetComponent<PlayerInput>();
        Controller = GetComponent<CharacterController>();

        // Setup
        Managers.Module.CreateModule(Module.LowerPosition, Module);
        CurrentLowerPart = Managers.Module.CurrentLowerPart;
        CurrentUpperPart = Managers.Module.CurrentUpperPart;

        Anim = GetComponentInChildren<Animator>();

        StateFactory = new PlayerStateFactory(this);
        CurrentState = StateFactory.Grounded();
        CurrentState.EnterState();

        // 초기 중력값 조절
        InitialGravity = Physics.gravity.y * 2;
    }

    private void Start()
    {
        // 콜백 함수 등록
        AddInputCallBacks();
    }

    private void Update()
    {
        HandleRotation();

        CurrentState.UpdateStates();

        HandleMove();
        HandleUseWeaponPrimary();        
    }

    private void AddInputCallBacks()
    {
        P_Input.Actions.Move.started += OnMovementInput;
        P_Input.Actions.Move.performed += OnMovementInput;
        P_Input.Actions.Move.canceled += OnMovementInput;
        P_Input.Actions.Jump.started += OnJump;
        P_Input.Actions.Jump.canceled += OnJump;

        P_Input.Actions.PrimaryWeapon.started += OnPrimaryWeapon;
        P_Input.Actions.PrimaryWeapon.canceled += OnPrimaryWeapon;
        P_Input.Actions.SecondaryWeapon.started += OnSecondaryWeapon;
        P_Input.Actions.SecondaryWeapon.canceled += OnSecondaryWeapon;
    }

    // InputAction 에 콜백 함수로 등록하여 입력값 받아옴. (이동관련)
    private void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovementDirection.x = _currentMovementInput.x;
        _currentMovementDirection.z = _currentMovementInput.y;
        IsMoveInputPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    // InputAction 에 콜백 함수로 등록하여 입력값 받아옴. (점프관련)
    private void OnJump(InputAction.CallbackContext context)
    {
        IsJumpInputPressed = context.ReadValueAsButton();
    }

    // InputAction 에 콜백 함수로 등록하여 입력값 받아옴. (전투관련)
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

    private void HandleMove()
    {
        _cameraRelativeMovement = ConvertToCameraSpace(_currentMovementDirection * _movementSpeed);

        //###### Debuging
        currentCamRelative = _cameraRelativeMovement;
        //######
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
        if (CurrentState is PlayerCombatState)
            CombatRotation();
        else
            NonCombatRotation();
    }

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
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _smoothRotateValue * Time.deltaTime);
        }
    }

    private void CombatRotation()
    {
        Vector3 positionToLookAt = Camera.main.transform.forward;
        positionToLookAt.y = 0;

        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);

        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _smoothRotateValue * Time.deltaTime);
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
}
