using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller
{
    protected Entity boss;

    public Vector3 Destination { get; protected set; }
    public Vector3 StopPoint { get; protected set; }

    public bool IsMoving { get; protected set; } = true;

    public float StopDistance { get; protected set; }

    public Rigidbody Rigidbody { get; protected set; }

    public void Initialize()
    {
        Destination = boss.transform.position;
        StopDistance = boss.Data.stopDistance;
        Rigidbody = boss.GetComponent<Rigidbody>();
    }

    public abstract void Update();

    protected abstract void Move();

    protected abstract void Look();

    public abstract void SetDestination(Vector3 target);

    public abstract void Stop();

    protected abstract void CheckDistance();
    
}
