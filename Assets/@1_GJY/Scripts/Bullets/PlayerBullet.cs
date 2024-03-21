using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : PlayerProjectile
{
    public override void Setup(float speed, Vector3 groundTargetPos, Transform target = null)
    {
        base.Setup(speed, groundTargetPos);

        _rigid.velocity = transform.forward * _speed;        
    }    
}
