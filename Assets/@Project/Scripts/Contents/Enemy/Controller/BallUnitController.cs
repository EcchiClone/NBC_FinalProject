using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallUnitController : Controller
{
    Vector3[] path;
    int targetIndex;

    Rigidbody rigidbody;
    Collider collider;

    float speed = 3;
    float _maxSpeed;
    float radius;
    float nodeRadius;


    public BallUnitController(Entity entity) : base(entity)
    {
        rigidbody = Entity.GetComponent<Rigidbody>();
        collider = Entity.GetComponent<Collider>();
        _maxSpeed = Entity.Data.moveSpeed;

        nodeRadius = PathRequestManager.instance.grid.nodeRadius;
        radius = collider.bounds.extents.magnitude;
    }

    public override void SetDestination(Vector3 target)
    {
        if(!IsMoving) IsMoving = true;
        PathRequestManager.RequestPath(Entity.transform.position, target, OnPathFound);
    }

    public override void SetStopDistance(float stopDistance)
    {
        if (0 > stopDistance)
            StopDistance = Entity.Data.stopDistance;
        else
            StopDistance = stopDistance;
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
        }
    }



    public override void Update()
    {
        if (!IsMoving) return;

        Move();
    }

    protected override void Move()
    {
        if (null == path || path.Length <= 0 )
        {
            return;
        }

        Vector3 currentWaypoint = path[0];
        currentWaypoint.y = radius;


        Vector3 direction = (currentWaypoint - Entity.transform.position).normalized;
        

        if (Mathf.Abs(Entity.transform.position.x - currentWaypoint.x) <= nodeRadius && Mathf.Abs(Entity.transform.position.z - currentWaypoint.z) <= nodeRadius)
        {
            ++targetIndex;
            if (targetIndex >= path.Length)
            {
                return;
            }
            currentWaypoint = path[targetIndex];
            currentWaypoint.y = radius;

            direction = (currentWaypoint - Entity.transform.position).normalized; // 노드 변경시 속도 방향 조정
            rigidbody.velocity = direction * rigidbody.velocity.magnitude;
        }

        rigidbody.AddForce(direction * speed, ForceMode.VelocityChange);

        if (rigidbody.velocity.magnitude > _maxSpeed)
            rigidbody.velocity = rigidbody.velocity.normalized * _maxSpeed;
    }

    public override void Stop() 
    {
        IsMoving = false;
    }

}
