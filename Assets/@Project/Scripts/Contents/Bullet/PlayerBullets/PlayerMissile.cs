using System.Collections;
using UnityEngine;

public class PlayerMissile : PlayerProjectile
{
    private bool _isTracking = false;
    private readonly float TRAKING_RATIO = 5f;
    private readonly float TRAKING_DELAY = 1f;

    public override void Setup(float speed, float damage, bool splash,Vector3 groundTargetPos, ITarget target = null, float explosiveRange = 0)
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
        yield return Util.GetWaitSeconds(TRAKING_DELAY);

        _isTracking = true;
    }
}
