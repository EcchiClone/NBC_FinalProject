using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Bullet
{
    // Temp
    [SerializeField] GameObject _hitEffectPrefab;

    [SerializeField] protected Define.BulletType _bulletType;

    protected Rigidbody _rigid;
    protected float _speed;
    protected float _damage;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();        
    }

    private IEnumerator Co_ReleaseBullet()
    {
        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }

    public virtual void Setup(float speed, float damage, Vector3 groundTargetPos, Transform target = null)
    {
        _speed = speed;
        _damage = damage;
        StartCoroutine(Co_ReleaseBullet());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Entity boss) == true)
        {
            boss.GetDamaged(_damage);
            StopAllCoroutines();

            if(_bulletType == Define.BulletType.Gun)
            {
                GameObject hitEffect = Instantiate(_hitEffectPrefab);
                hitEffect.transform.position = transform.position;
                hitEffect.transform.rotation = transform.rotation;
            }            

            Destroy(gameObject);
        }
    }
}
