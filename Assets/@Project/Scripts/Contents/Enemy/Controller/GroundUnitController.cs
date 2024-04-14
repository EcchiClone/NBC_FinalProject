using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundUnitController : Controller
{
    private NavMeshAgent agent;

    // Must Fix This.
    Transform head;


    public GroundUnitController(Entity entity) : base(entity)
    {
        agent = entity.GetComponent<NavMeshAgent>();
        agent.speed = entity.Data.moveSpeed;
        agent.stoppingDistance = entity.Data.stopDistance;

        // Must Fix This.
        head = entity.transform.Find("Armature/Body/Head");
    }

    public override void Update()
    {
        IsMoving = !(agent.velocity.magnitude < 0.1f);
        Look();
    }

    public override void SetDestination(Vector3 target)
    {
        agent.SetDestination(target);
    }

    public override void SetStopDistance(float stopDistance)
    {
        if(0 > stopDistance)
            agent.stoppingDistance = StopDistance = _entity.Data.stopDistance;
        else
            agent.stoppingDistance = StopDistance = stopDistance;
    }

    protected override void Look()
    {
        Quaternion currentLocalRotation = head.localRotation;
        head.localRotation = Quaternion.identity;
        Vector3 targetWorldLookDir = Target.position - head.position;
        Vector3 targetLocalLookDir = head.InverseTransformDirection(targetWorldLookDir);

        Quaternion targetLocalRotation = Quaternion.LookRotation(targetLocalLookDir, Vector3.up);

        // 쿼터니언 -> 오일러 각도로 변경
        Vector3 euler = targetLocalRotation.eulerAngles;
        euler.x = 0;
        euler.z = 0;

        // 오일러 -> 쿼터니언으로 변경
        targetLocalRotation = Quaternion.Euler(euler);

        head.localRotation = Quaternion.Slerp(
            currentLocalRotation,
            targetLocalRotation,
            1 - Mathf.Exp(-_entity.Data.rotationSpeed * Time.deltaTime)
            );
    }

    public override void Stop()
    {
        agent.isStopped = true;
    }

}
