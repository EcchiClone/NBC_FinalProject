using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayerHETAShell : PlayerProjectile
{
    public override void Setup(float speed, float damage, bool splash, Vector3 groundTargetPos, Transform target = null, float explosiveRange = 0)
    {
        base.Setup(speed, damage, splash, groundTargetPos);

        _rigid.velocity = transform.forward * _speed;
    }

    private void Update()
    {
        // 중력효과
    }
}
