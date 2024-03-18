using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    protected Rigidbody _rigid;
    protected float _speed;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        Destroy(gameObject, 5f);
    }

    public virtual void Setup(float speed, Vector3 groundTargetPos, Transform target = null)
    {
        _speed = speed;
    }   
}
