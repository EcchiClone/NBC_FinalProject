using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Bullet
{
    // Temp
    [SerializeField] GameObject _hitEffectPrefab;

    [SerializeField] protected Define.BulletType _bulletType;
    [SerializeField] protected LayerMask _damagableLayer;

    protected Rigidbody _rigid;
    protected float _speed;
    protected float _damage;

    protected bool _isSplash;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    private IEnumerator Co_ReleaseBullet()
    {
        yield return Util.GetWaitSeconds(5f);

        Destroy(gameObject);
    }

    public virtual void Setup(float speed, float damage, bool splash, Vector3 groundTargetPos, Transform target = null)
    {
        _speed = speed;
        _damage = damage;
        _isSplash = splash;
        StartCoroutine(Co_ReleaseBullet());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Entity boss) == true)
        {
            boss.GetDamaged(_damage);
            StopAllCoroutines();

            if (_bulletType == Define.BulletType.Gun)
            {
                GameObject hitEffect = Instantiate(_hitEffectPrefab);
                hitEffect.transform.position = transform.position;
                hitEffect.transform.rotation = transform.rotation;
            }

            Destroy(gameObject);
        }
        // Test
        else if ((_damagableLayer & (1 << other.gameObject.layer)) != 0)
        {
            StopAllCoroutines();

            GameObject hitEffect = Instantiate(_hitEffectPrefab);
            hitEffect.transform.position = transform.position;
            hitEffect.transform.rotation = transform.rotation;

            // To Do - 스플래시 여부에 따라 주변 데미지도 생각 해야함.
            if (_isSplash)
            {
                RaycastHit[] hits = Physics.SphereCastAll(transform.position, 5/*범위지정*/, Vector3.up, 0, _damagableLayer);

                foreach (var hit in hits) // 범위에 들어간 적은 데미지 부여
                {
                    if (hit.transform.TryGetComponent(out Test_Enemy testEnemy))
                    {
                        // To Do - 데미지 
                        Debug.Log("스플뎀");
                        testEnemy.GetDamage(_damage);
                    }
                }
            }
            else
            {
                if (other.TryGetComponent(out Test_Enemy testEnemy))
                {
                    // To Do - 단순 타겟 데미지
                    Debug.Log("일반뎀");
                    testEnemy.GetDamage(_damage);
                }                    
            }

            gameObject.SetActive(false);
        }        
    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject); // 한 객체에 한번만
        CancelInvoke();
    }
}
