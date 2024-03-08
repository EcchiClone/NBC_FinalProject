using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("# Test")]
    [Range(2f, 10f)][SerializeField] float _movementSpeed;
    [Range(5f, 15f)][SerializeField] float _smoothRotateValue;
    [field: Range(0.1f, 1f)] [field: SerializeField] public float MinDownForceValue { get; private set; } // 접지 중일 때 최소 중력배율
    [field: Range(1f, 100f)] [field: SerializeField] public float JumpPower { get; private set; } // 접지 중일 때 최소 중력배율


    [field: Header("# Animation")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    public PlayerInput Input { get; private set; }
    public CharacterController Controller { get; private set; }
    public Animator Anim { get; private set; }

    public float InitialGravity { get; private set; }

    public bool IsMoveInputPressed { get; private set; }
    public bool IsJumpInputPressed { get; private set; }

    public bool IsJumping { get; set; }

    public Vector2 _currentMovementInput;
    public Vector3 _currentMovementDirection;

    public PlayerBaseState CurrentSuperState { get; set; }    
    public PlayerStateFactory StateFactory { get; private set; }

    private void Awake()
    {
        // 애니메이션 Hash 초기화
        AnimationData.Init();

        // 컴포넌트 Get
        Input = GetComponent<PlayerInput>();
        Controller = GetComponent<CharacterController>();
        Anim = GetComponentInChildren<Animator>();

        // Setup State
        StateFactory = new PlayerStateFactory(this);
        CurrentSuperState = StateFactory.Grounded();
        CurrentSuperState.EnterState();        

        // 콜백 함수 등록
        AddInputCallBacks();

        // 초기 중력값 조절
        InitialGravity = Physics.gravity.y * 2;
    }

    private void Update()
    {
        HandleRotation();

        CurrentSuperState.UpdateStates();
        Controller.Move(_currentMovementDirection * _movementSpeed * Time.deltaTime);        
    }

    private void AddInputCallBacks()
    {
        Input.Actions.Move.started += OnMovementInput;
        Input.Actions.Move.performed += OnMovementInput;
        Input.Actions.Move.canceled += OnMovementInput;
        Input.Actions.Jump.started += OnJump;
        Input.Actions.Jump.canceled += OnJump;
    }

    // Temp - InputAction 에 콜백 함수로 등록하여 입력값 받아옴. (이동관련)
    private void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovementDirection.x = _currentMovementInput.x;
        _currentMovementDirection.z = _currentMovementInput.y;
        IsMoveInputPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    // Temp - InputAction 에 콜백 함수로 등록하여 입력값 받아옴. (점프관련)
    private void OnJump(InputAction.CallbackContext context)
    {
        IsJumpInputPressed = context.ReadValueAsButton();
    }

    // Temp - 회전 제어
    private void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = _currentMovementDirection.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = _currentMovementDirection.z;

        Quaternion currentRotation = transform.rotation;

        if (IsMoveInputPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _smoothRotateValue * Time.deltaTime);
        }
    }
}
