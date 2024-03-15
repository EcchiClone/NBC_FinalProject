using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : PlayerProjectile
{
    public override void Setup(Transform target, float speed, Vector3 groundTargetPos)
    {
        base.Setup(target, speed, groundTargetPos);

        _rigid.velocity = transform.forward * _speed;
        Destroy(gameObject, 5f);
    }    
}
