using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissile : PlayerProjectile
{
    private Transform _target;
    private Vector3 _groundTargetPos;
    private WaitForSeconds _trackingDealy = new WaitForSeconds(1f);

    private bool _isTracking = false;
    private readonly float TRAKING_RATIO = 5f;

    public override void Setup(Transform target, float speed, Vector3 groundTargetPos)
    {
        base.Setup(target, speed, groundTargetPos);
        _target = target;
        _groundTargetPos = groundTargetPos;
        StartCoroutine(CoTracking());
    }

    private void Update()
    {
        if (_isTracking)
        {
            if (_target != null)
                TrackingTarget(_target.position);
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
        yield return _trackingDealy;

        _isTracking = true;
    }
}
