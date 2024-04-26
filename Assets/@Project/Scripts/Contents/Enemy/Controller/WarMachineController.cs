using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarMachineController : Controller
{
    private NavMeshAgent agent;

    private Transform _body;

    public WarMachineController(Entity entity) : base(entity)
    {
        agent = entity.GetComponent<NavMeshAgent>();
        agent.speed = entity.FinalMoveSPD;
        agent.angularSpeed = entity.Stat.rotationSpeed;
        agent.stoppingDistance = entity.Stat.stopDistance -1;

        _body = Entity.Transform;
    }

    public override void Update()
    {
        IsMoving = (agent.velocity.magnitude > 0.1f);
        Look();

        if (agent.velocity.sqrMagnitude >= 0.01f && true != Entity.Anim.GetBool("Walk")) // 속도 0.1 이상 && Walk false
        {
            Entity.Anim.SetBool("Walk", true);
        }
        else if (agent.velocity.sqrMagnitude < 0.01f && true == Entity.Anim.GetBool("Walk"))
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
        if (0 > stopDistance)
            agent.stoppingDistance = StopDistance = Entity.Stat.stopDistance;
        else
            agent.stoppingDistance = StopDistance = stopDistance;
    }

    protected override void Look()
    {
        if (null == _body)
            return;

        Quaternion currentLocalRotation = _body.localRotation;
        _body.localRotation = Quaternion.identity;
        Vector3 targetWorldLookDir = Target.position - _body.position;
        Vector3 targetLocalLookDir = _body.InverseTransformDirection(targetWorldLookDir);

        Quaternion targetLocalRotation = Quaternion.LookRotation(targetLocalLookDir, Vector3.up);

        // Quaternion -> Euler
        Vector3 euler = targetLocalRotation.eulerAngles;
        euler.x = 0;
        euler.z = 0;

        // Euler -> Quaternion
        targetLocalRotation = Quaternion.Euler(euler);

        _body.localRotation = Quaternion.Slerp(
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
