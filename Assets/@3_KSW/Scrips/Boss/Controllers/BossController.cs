using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossController
{
    protected Boss boss;

    protected Vector3 destination;
    protected Vector3 stopPoint;

    protected float stopDistance;

    public bool IsMoving { get; protected set; } = true;

    public Vector3 Destination { 
        get { return destination; } 
        set { destination = value; }
    }

    public float StopDistance 
    {
        get { return stopDistance; }
        set { stopDistance = value; }
    }

    public void Initialize()
    {
        destination = boss.transform.position;
        stopDistance = boss.Data.stopDistance;
    }

    public abstract void Update();

    protected abstract void Move();

    protected abstract void Look();

    public abstract void SetDestination(Vector3 target);

    public abstract void Stop();

    protected abstract void CheckDistance();
    
}
