using UnityEngine;
using UnityEngine.AI;

public class GroundUnitController : Controller
{
    private NavMeshAgent agent;

    // Must Fix This.
    private Transform _head;
    private Transform _weapon;
    

    public GroundUnitController(Entity entity) : base(entity)
    {
        agent = entity.GetComponent<NavMeshAgent>();
        agent.speed = entity.FinalMoveSPD;
        agent.angularSpeed = entity.Stat.rotationSpeed;
        agent.stoppingDistance = entity.Stat.stopDistance -1;

        // Must Fix This.
        _head = entity.transform.Find("Armature/Body/Head");
        _weapon = entity.transform.Find("Armature/Body/Head/Weapon");
    }

    public override void Update()
    {
        IsMoving = (agent.velocity.magnitude > 0.1f);
        Look();

        if(agent.velocity.sqrMagnitude >= 0.01f && true != Entity.Anim.GetBool("Walk")) // 속도 0.1 이상 && Walk false
        {
            Entity.Anim.SetBool("Walk", true);
        }
        else if(agent.velocity.sqrMagnitude < 0.01f && true == Entity.Anim.GetBool("Walk"))
        {
            Entity.Anim.SetBool("Walk", false);
        }
    }

    public override void SetDestination(Vector3 target)
    {
        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            agent.SetDestination(target);            
        }
        else
        {
            Debug.Log("네비 매쉬 오류입니다.");
            return;
        }
    }

    public override void SetStopDistance(float stopDistance)
    {
        if(0 > stopDistance)
            agent.stoppingDistance = StopDistance = Entity.Stat.stopDistance;
        else
            agent.stoppingDistance = StopDistance = stopDistance;
    }

    protected override void Look()
    {
        if (null == _head || null == _weapon)
            return;

        Quaternion currentLocalRotation = _head.localRotation;
        _head.localRotation = Quaternion.identity;
        Vector3 targetWorldLookDir = Target.position - _head.position;
        Vector3 targetLocalLookDir = _head.InverseTransformDirection(targetWorldLookDir);

        Quaternion targetLocalRotation = Quaternion.LookRotation(targetLocalLookDir, Vector3.up);

        // Quaternion -> Euler
        Vector3 euler = targetLocalRotation.eulerAngles;
        euler.x = 0;
        euler.z = 0;

        // Euler -> Quaternion
        targetLocalRotation = Quaternion.Euler(euler);

        _head.localRotation = Quaternion.Slerp(
            currentLocalRotation,
            targetLocalRotation,
            1 - Mathf.Exp(-Entity.Stat.rotationSpeed * Time.deltaTime)
            );


        currentLocalRotation = _weapon.localRotation;
        _weapon.localRotation = Quaternion.identity;
        targetWorldLookDir = Target.position - _weapon.position;
        targetLocalLookDir = _weapon.InverseTransformDirection(targetWorldLookDir);
        targetLocalRotation = Quaternion.LookRotation(targetLocalLookDir, Vector3.up);

        euler = targetLocalRotation.eulerAngles;
        euler.y = 0;
        euler.z = 0;

        targetLocalRotation = Quaternion.Euler(euler);

        _weapon.localRotation = Quaternion.Slerp(
            currentLocalRotation,
            targetLocalRotation,
            1 - Mathf.Exp(-Entity.Stat.rotationSpeed * Time.deltaTime)
            );
    }

    public override void Stop()
    {
        agent.isStopped = true;
    }

}
