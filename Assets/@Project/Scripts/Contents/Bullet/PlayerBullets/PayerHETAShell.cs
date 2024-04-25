using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayerHETAShell : PlayerProjectile
{
    public override void Setup(float speed, float damage, bool splash, Vector3 groundTargetPos, ITarget target = null, float explosiveRange = 0)
    {
        base.Setup(speed, damage, splash, groundTargetPos, target, explosiveRange);

        _rigid.velocity = transform.forward * _speed;
    }
}
