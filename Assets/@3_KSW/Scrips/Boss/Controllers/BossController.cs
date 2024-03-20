using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossController
{
    protected Boss boss;

    protected Vector3 destination;

    protected float stopDistance;

    protected bool isMoving = true;

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
        stopDistance = boss.data.stopDistance;
    }

    public abstract void Update();

    public abstract void Move();

    public abstract void SetDestination(Vector3 target);

    public abstract void Stop();

    protected abstract void CheckDistance();
    
}
