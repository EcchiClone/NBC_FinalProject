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
        rigidbody = _entity.GetComponent<Rigidbody>();
        collider = _entity.GetComponent<Collider>();
        _maxSpeed = _entity.Data.moveSpeed;

        nodeRadius = PathRequestManager.instance.grid.nodeRadius;
        radius = collider.bounds.extents.magnitude;
    }

    public override void SetDestination(Vector3 target)
    {
        if(!IsMoving) IsMoving = true;
        PathRequestManager.RequestPath(_entity.transform.position, target, OnPathFound);
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

        Vector3 currentWaypoint = path[0]; // TODO : 수정 필요 -> 코루틴에서만 유효한 방식임 멤버변수화 시켜야함
        currentWaypoint.y = radius;


        Vector3 direction = (currentWaypoint - _entity.transform.position).normalized; // 다음 노드로 속도 방향 조정      
                

        if (Mathf.Abs(_entity.transform.position.x - currentWaypoint.x) <= nodeRadius && Mathf.Abs(_entity.transform.position.z - currentWaypoint.z) <= nodeRadius)
        {
            ++targetIndex;
            if (targetIndex >= path.Length)
            {
                return;
            }
            currentWaypoint = path[targetIndex];
            currentWaypoint.y = radius;

            direction = (currentWaypoint - _entity.transform.position).normalized; // 노드 변경시 속도 방향 조정

            rigidbody.velocity = direction * rigidbody.velocity.magnitude;
        }


        rigidbody.AddForce(direction * speed, ForceMode.Force);
        //rigidbody.velocity = direction * rigidbody.velocity.magnitude;

        if (rigidbody.velocity.magnitude > _maxSpeed)
            rigidbody.velocity = rigidbody.velocity.normalized * _maxSpeed;

        //transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);

    }

    public override void Stop() 
    {
        IsMoving = false;
    }


    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(_entity.transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
