using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Bullet
{
    protected Rigidbody _rigid;
    protected float _speed;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();        
    }

    private IEnumerator Co_ReleaseBullet()
    {
        yield return new WaitForSeconds(5);

        EnemyBulletPoolManager.instance.OnReturnedToPool(gameObject);
    }

    public virtual void Setup(float speed, Vector3 groundTargetPos, Transform target = null)
    {
        _speed = speed;
        StartCoroutine(Co_ReleaseBullet());
    }

    public void HitTarget()
    {
        
    }
}
