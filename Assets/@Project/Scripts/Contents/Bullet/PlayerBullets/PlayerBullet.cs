using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : PlayerProjectile
{
    public override void Setup(float speed, float damage, Vector3 groundTargetPos, Transform target = null)
    {
        base.Setup(speed, damage, groundTargetPos);

        _rigid.velocity = transform.forward * _speed;        
    }    
}
