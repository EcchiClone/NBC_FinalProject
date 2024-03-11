using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("# Test")]
    [Range(2f, 10f)][SerializeField] float _movementSpeed;
    [Range(5f, 15f)][SerializeField] float _smoothRotateValue;
    [Range(0.1f, 0.5f)][SerializeField] float _minDownForceValue; // 접지 중일 때 최소 중력배율
    [Range(1f, 100f)][SerializeField] float _jumpPower; // 접지 중일 때 최소 중력배율

    [field: Header("# Animation")]
    [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

    public PlayerInput Input { get; private set; }
    public CharacterController Controller { get; private set; }
    public Animator Anim { get; private set; }

    private Vector2 _currentMovementInput;
    private Vector3 _currentMovementDirection;
    private bool _isMoveInputPressed;
    private bool _isJumpInputPressed;

    private float _initialGravity;
    private bool _isJumping = false;

    private void Awake()
    {
        AnimationData.Init();

        Input = GetComponent<PlayerInput>();
        Controller = GetComponent<CharacterController>();
        Anim = GetComponentInChildren<Animator>();

        AddInputCallBacks();

        _initialGravity = Physics.gravity.y * 2;
    }

    private void Update()
    {
        HandleRotation();
        HandleAnimation();
        Move(); // Q. 물리적 이동은 FixedUpdate로 쓰이는 줄 알았는데 왜 Update에서 쓰는 걸까?
        HandleGravity();
        HandleJump();
    }

    private void AddInputCallBacks()
    {
        Input.Actions.Move.started += OnMovementInput;
        Input.Actions.Move.performed += OnMovementInput;
        Input.Actions.Move.canceled += OnMovementInput;
        Input.Actions.Jump.started += OnJump;
        Input.Actions.Jump.canceled += OnJump;
    }

    // Temp - 실제 이동에 쓰일 함수
    private void Move()
    {
        Controller.Move(_currentMovementDirection * _movementSpeed * Time.deltaTime);
    }

    // Temp - InputAction 에 콜백 함수로 등록하여 입력값 받아옴. (이동관련)
    private void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovementDirection.x = _currentMovementInput.x;
        _currentMovementDirection.z = _currentMovementInput.y;
        _isMoveInputPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    // Temp - InputAction 에 콜백 함수로 등록하여 입력값 받아옴. (점프관련)
    private void OnJump(InputAction.CallbackContext context)
    {
        _isJumpInputPressed = context.ReadValueAsButton();
    }

    // Temp - 점프
    private void HandleJump()
    {
        if (_isJumpInputPressed && Controller.isGrounded && !_isJumping)
        {
            _isJumping = true;
            _currentMovementDirection.y = _jumpPower;
        }
        else if (!_isJumpInputPressed && !Controller.isGrounded && _isJumping)
            _isJumping = false;
    }

    // Temp - CharacterController 컴포넌트는 중력에 대해 연산 안 하기 때문에 직접 조정 해줘야 함.
    private void HandleGravity()
    {
        if (Controller.isGrounded)
        {
            _currentMovementDirection.y = -_minDownForceValue;
        }
        else
        {
            _currentMovementDirection.y += _initialGravity * Time.deltaTime;
        }
    }

    // Temp - 회전 제어
    private void HandleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = _currentMovementDirection.x;
        positionToLookAt.y = 0f;
        positionToLookAt.z = _currentMovementDirection.z;

        Quaternion currentRotation = transform.rotation;

        if (_isMoveInputPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _smoothRotateValue * Time.deltaTime);
        }
    }

    // Temp - 애니메이션 제어 : 현재 Walk만 가능
    private void HandleAnimation()
    {
        bool isWalking = Anim.GetBool(AnimationData.WalkParameterHash);

        if (_isMoveInputPressed && !isWalking)
        {
            Anim.SetBool(AnimationData.WalkParameterHash, true);
        }
        else if (!_isMoveInputPressed && isWalking)
        {
            Anim.SetBool(AnimationData.WalkParameterHash, false);
        }
    }
}
