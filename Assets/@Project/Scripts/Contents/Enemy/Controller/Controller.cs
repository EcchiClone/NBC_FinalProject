using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Controller
{
    public Controller(Entity entity)
    {
        this._entity = entity;
    }

    protected Entity _entity;

    public Vector3 Destination { get; protected set; }
    public Vector3 StopPoint { get; protected set; }

    public bool IsMoving { get; set; } = true;

    public float StopDistance { get; protected set; }

    public Rigidbody Rigidbody { get; protected set; }

    protected Transform Target { get; set; }

    public void Initialize()
    {
        Destination = _entity.transform.position;
        StopDistance = _entity.Data.stopDistance;
        Rigidbody = _entity.GetComponent<Rigidbody>();
        Target = _entity.Target.transform;
    }

    public virtual void Update() { }

    protected virtual void Move() { }

    protected virtual void Look() { }

    public virtual void SetDestination(Vector3 target) { }

    public virtual void Stop() { }

    protected virtual void CheckDistance() { }
    
}
