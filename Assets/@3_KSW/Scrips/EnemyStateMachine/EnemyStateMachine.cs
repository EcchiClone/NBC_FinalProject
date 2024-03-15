using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    private Rigidbody rigid;
    private EnemyBaseState _currentState;
    private EnemyStateFactory _states;
    public Transform target;
    private NavMeshAgent _nma;

    private float _shootDistance = 30f;
    private float _stopDistance = 20f;
    private float _runDistance = 10f;
    private float _moveSpeed = 10f;
    public float gizmoLength = 10f; // 기즈모 길이

    #region 삭제하세용
    [Header("# Test 꼭 수정하시길...")]
    public PhaseSO currentPhase;

    public bool isShooting = true;  // false가 되면 DanmakuGenerator에서 생성 작업을 멈춘다.
                                    // 사이클 돌리던 코루틴이 완전히 멈추는 것이므로 StartPatternHierarchy 를 다시 해줘야 함.
    public bool[] isTriggerOn;      // 탄막 일괄 이벤트를 위한 트리거를 위해 추후 준비.
    #endregion



    public EnemyBaseState CurrentState
    {
        get { return _currentState; }
        set { _currentState = value; }
    }
    public EnemyStateFactory States { get { return _states; } }
    public Transform Target { get { return target; } }
    public NavMeshAgent Nma { get { return _nma; } }

    public float StoppingDistance { get { return _stopDistance; } }


    Color gizmoColor = Color.green;

    //조건
    private bool _isShoot = false;
    private bool _isChasing = false;
    private bool _isStandoff = false;
    private bool _isRun = false;

    public bool IsChasing {  get { return _isChasing; } }
    public bool IsStandoff {  get { return _isStandoff; } }
    public bool IsRun { get { return _isRun; } }
    

    public Color GizmoColor { set { gizmoColor = value; } }


    private void Awake()
    {
        rigid = gameObject.GetComponent<Rigidbody>();

        CheckDistance();

        _states = new EnemyStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        _nma = gameObject.GetOrAddComponent<NavMeshAgent>();
        _nma.updateRotation = false;
        _nma.stoppingDistance = _stopDistance;

    }


    void Start()
    {
        target = FindObjectOfType<PlayerStateMachine>().transform;
    }


    void Update()
    {
        CheckDistance();

        _currentState.UpdateStates();
    }

    private void FixedUpdate()
    {
        FreezeVelocity();
    }

    private void CheckDistance()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget < _shootDistance)
            _isShoot = true;
        else 
            _isShoot = false;

        if(distanceToTarget > _stopDistance)
        {
            _isChasing = true;
            _isStandoff = false;
            _isRun = false;
        }
        else if(distanceToTarget <= _stopDistance && distanceToTarget > _runDistance)
        {
            _isChasing = false;
            _isStandoff = true;
            _isRun = false;
        }
        else if(distanceToTarget <= _runDistance)
        {
            _isChasing = false;
            _isStandoff = false;
            _isRun = true;
        }
    }

    void FreezeVelocity()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;   
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        Gizmos.DrawRay(transform.position, transform.forward * gizmoLength);
    }
}