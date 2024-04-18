using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class GroundUnitController : Controller
{
    private NavMeshAgent agent;

    // Must Fix This.
    Transform head;
    Transform weapon;
    

    public GroundUnitController(Entity entity) : base(entity)
    {
        agent = entity.GetComponent<NavMeshAgent>();
        agent.speed = entity.Data.moveSpeed;
        agent.stoppingDistance = entity.Data.stopDistance;

        // Must Fix This.
        head = entity.transform.Find("Armature/Body/Head");
        weapon = entity.transform.Find("Armature/Body/Head/Weapon");


    }

    public override void Update()
    {
        IsMoving = (agent.velocity.magnitude > 0.1f);
        Look();

        if(agent.velocity.sqrMagnitude > 0.01f && !Entity.Anim.GetBool("Walk")) // 속도 0.1 이상 && Walk false
        {
            Entity.Anim.SetBool("Walk", true);
        }
        else if(agent.velocity.sqrMagnitude < 0.01f && Entity.Anim.GetBool("Walk"))
        {
            Entity.Anim.SetBool("Walk", false);
        }
    }

    public override void SetDestination(Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();

        if (agent.isActiveAndEnabled && agent.isOnNavMesh && agent.CalculatePath(target, path))
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
            agent.stoppingDistance = StopDistance = Entity.Data.stopDistance;
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

        // Quaternion -> Euler
        Vector3 euler = targetLocalRotation.eulerAngles;
        euler.x = 0;
        euler.z = 0;

        // Euler -> Quaternion
        targetLocalRotation = Quaternion.Euler(euler);

        head.localRotation = Quaternion.Slerp(
            currentLocalRotation,
            targetLocalRotation,
            1 - Mathf.Exp(-Entity.Data.rotationSpeed * Time.deltaTime)
            );


        currentLocalRotation = weapon.localRotation;
        weapon.localRotation = Quaternion.identity;
        targetWorldLookDir = Target.position - weapon.position;
        targetLocalLookDir = weapon.InverseTransformDirection(targetWorldLookDir);
        targetLocalRotation = Quaternion.LookRotation(targetLocalLookDir, Vector3.up);

        euler = targetLocalRotation.eulerAngles;
        euler.y = 0;
        euler.z = 0;

        targetLocalRotation = Quaternion.Euler(euler);

        weapon.localRotation = Quaternion.Slerp(
            currentLocalRotation,
            targetLocalRotation,
            1 - Mathf.Exp(-Entity.Data.rotationSpeed * Time.deltaTime)
            );
    }

    public override void Stop()
    {
        agent.isStopped = true;
    }

}
