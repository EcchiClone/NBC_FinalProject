using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHellFire : PlayerProjectile
{
    private bool _isTracking = false;
    private readonly float TRAKING_RATIO = 5f;

    public override void Setup(float speed, float damage, bool splash, Vector3 groundTargetPos, ITarget target = null, float explosiveRange = 0)
    {
        base.Setup(speed, damage, splash, groundTargetPos, target, explosiveRange);
        _isTracking = false;        
        StartCoroutine(CoTracking());
    }

    private void Update()
    {
        if (_isTracking)
        {
            if (_target != null)
                TrackingTarget(_target.Transform.position);
            else
                TrackingTarget(_groundTargetPos);
        }

        _rigid.velocity = transform.forward * _speed;
    }

    private void TrackingTarget(Vector3 targetPos)
    {
        Vector3 direction = targetPos - transform.position;
        Quaternion targetRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, TRAKING_RATIO * Time.deltaTime);
    }

    private IEnumerator CoTracking()
    {
        yield return Util.GetWaitSeconds(1f);

        _isTracking = true;
    }
}
