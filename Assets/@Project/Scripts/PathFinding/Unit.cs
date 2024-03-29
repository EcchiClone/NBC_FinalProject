using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    float speed = 3;
    Vector3[] path;
    int targetIndex;

    Rigidbody rigidbody;
    Collider collider;
    float _maxSpeed = 10;
    float radius;

    Grid grid;

    float passedTime = 0f;
    float interval = 0.2f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        grid = PathRequestManager.instance.grid;
        radius = collider.bounds.extents.magnitude;
    }

    private void Update()
    {
        if(passedTime >= interval)
        {
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            passedTime = 0f;
        }
            passedTime += Time.deltaTime;
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    private void FixedUpdate()
    {
        /*Vector3 currentWaypoint = path[0];
        while (path.Length >= 1)
        {
            currentWaypoint.y = transform.position.y;
            Vector3 direction = (currentWaypoint - transform.position).normalized;

            if (Mathf.Abs(transform.position.x - currentWaypoint.x) <= grid.nodeRadius && Mathf.Abs(transform.position.z - currentWaypoint.z) <= grid.nodeRadius)
            {
                ++targetIndex;
                if (targetIndex >= path.Length)
                {
                    break;
                }
                currentWaypoint = path[targetIndex];
            }

            rigidbody.AddForce(direction * speed, ForceMode.Force);
        }*/
    }

    IEnumerator FollowPath() // 이동
    {
        //targetIndex = 0;
        if (path.Length <= 0)
        {
            yield break;
        }

        Vector3 currentWaypoint = path[0];
        currentWaypoint.y = radius;
        while (true)
        {
            Vector3 direction = (currentWaypoint - transform.position).normalized; // 다음 노드로 속도 방향 조정      

            if (Mathf.Abs(transform.position.x - currentWaypoint.x) <= grid.nodeRadius && Mathf.Abs(transform.position.z - currentWaypoint.z) <= grid.nodeRadius)
            {
                ++targetIndex;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
                currentWaypoint.y = radius;

                direction = (currentWaypoint - transform.position).normalized; // 노드 변경시 속도 방향 조정

                rigidbody.velocity = direction * rigidbody.velocity.magnitude;
            }


            rigidbody.AddForce(direction * speed, ForceMode.Force);
            rigidbody.velocity = direction * rigidbody.velocity.magnitude;

            if (rigidbody.velocity.magnitude > _maxSpeed)
                rigidbody.velocity = rigidbody.velocity.normalized * _maxSpeed;

            //transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;

        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
