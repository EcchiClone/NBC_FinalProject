using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundUnitController : Controller
{
    private NavMeshAgent agent;

    public GroundUnitController(Entity entity) : base(entity)
    {
        agent = entity.GetComponent<NavMeshAgent>();
    }

    public override void Update()
    {
        IsMoving = !agent.isStopped;
        Look();
    }

    public override void SetDestination(Vector3 target) => agent.SetDestination(target);
    
    protected override void Look()
    {
        Vector3 direction = (_entity.Target.position - _entity.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        _entity.transform.rotation = Quaternion.RotateTowards(_entity.transform.rotation, lookRotation, 180 * Time.deltaTime);
    }

    public override void Stop() => agent.Stop();

}
