using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Controller
{
    public Controller(Entity entity)
    {
        this.Entity = entity;
    }

    protected Entity Entity { get; set; }

    public Vector3 Destination { get; protected set; }
    public Vector3 StopPoint { get; protected set; }

    public bool IsMoving { get; set; } = true;
    public bool IsChasing { get; set; } = false;

    public float StopDistance { get; protected set; }

    public Rigidbody Rigidbody { get; protected set; }

    protected Transform Target { get; set; }

    public void Initialize()
    {
        Destination = Entity.transform.position;
        StopDistance = Entity.Data.stopDistance;
        Rigidbody = Entity.GetComponent<Rigidbody>();
        Target = Entity.Target.transform;
    }

    public virtual void Update() { }

    protected virtual void Move() { }

    protected virtual void Look() { }

    public virtual void SetDestination(Vector3 target) { }

    /// <param name="stopDistance">0 미만일 경우 원래의 StopDistance 적용</param>
    public virtual void SetStopDistance(float stopDistance = -1f) { }

    public virtual void Stop() { }

    protected virtual void CheckDistance() { }
    
}
