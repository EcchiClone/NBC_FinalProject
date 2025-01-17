using System.Collections;
using UnityEngine;

public class PlayerProjectile : Bullet
{
    [SerializeField] GameObject _hitEffectPrefab;
    [SerializeField] TrailRenderer _trailRenderers;
    [SerializeField] protected Define.BulletType _bulletType;
    [SerializeField] protected Define.BulletHitSounds _sfxType;
    [SerializeField] protected LayerMask _damagableLayer;

    protected ITarget _target;
    protected Vector3 _groundTargetPos;

    protected Rigidbody _rigid;
    protected float _speed;
    protected float _damage;
    protected float _explosiveRange;

    protected bool _isSplash;

    private Coroutine _coroutine;
    private readonly float LIFE_TIME = 5f;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    public virtual void Setup(float speed, float damage, bool splash, Vector3 groundTargetPos, ITarget target = null, float explosiveRange = 0)
    {
        _speed = speed;
        _damage = damage;
        _isSplash = splash;
        _explosiveRange = explosiveRange;
        _groundTargetPos = groundTargetPos;
        _target = target;

        if (_trailRenderers != null)
            _trailRenderers.Clear();

        _coroutine = StartCoroutine(Co_LifeTime());
    }

    private IEnumerator Co_LifeTime()
    {
        yield return Util.GetWaitSeconds(LIFE_TIME);

        if (_isSplash)
            Explode();        
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((_damagableLayer & (1 << collision.gameObject.layer)) != 0)
        {
            if (_isSplash)
                Explode();
            else
            {
                StopAllCoroutines();
                CreateEffect();
                PlaySFX();

                if (collision.gameObject.TryGetComponent(out ITarget target))
                    target.GetDamaged(_damage);

                else if (collision.gameObject.TryGetComponent(out DummyController dummy))
                    dummy.GetDamaged(_damage, transform.position);
            }
            gameObject.SetActive(false);
        }
    }

    protected void Explode()
    {
        StopAllCoroutines();
        CreateEffect();
        PlaySFX();

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, _explosiveRange, Vector3.up, 0, _damagableLayer);

        foreach (var hit in hits) // 범위에 들어간 적은 데미지 부여
        {
            if (hit.transform.TryGetComponent(out ITarget target))
                target.GetDamaged(_damage);

            else if (hit.transform.TryGetComponent(out DummyController dummy))
                dummy.GetDamaged(_damage, transform.position);
        }
    }

    private void PlaySFX()
    {
        switch (_sfxType)
        {            
            case Define.BulletHitSounds.CannonSmall:
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Bullet_Hit, transform.position);
                break;
            case Define.BulletHitSounds.Gatling:
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Bullet_Hit, transform.position);
                break;
            case Define.BulletHitSounds.Laser:

                break;
            case Define.BulletHitSounds.CannonLarge:
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Bullet_Hit, transform.position);
                break;
            case Define.BulletHitSounds.Missile:
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Explosion_Small, transform.position);
                break;
            case Define.BulletHitSounds.HellFire:
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Explosion_Small, transform.position);
                break;
            case Define.BulletHitSounds.HECannon:
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Explosion_Big, transform.position);
                break;
        }
    }

    private void CreateEffect()
    {
        GameObject go = Managers.Pool.GetPooler(PoolingType.Player).SpawnFromPool(_hitEffectPrefab.name, transform.position);
        EffectLifeTime hitEffect = go.GetComponent<EffectLifeTime>();
        hitEffect.Setup();
        hitEffect.transform.rotation = transform.rotation;
    }

    protected virtual void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        Managers.Pool.GetPooler(PoolingType.Player).ReturnToPool(gameObject); // 한 객체에 한번만        
        CancelInvoke();
    }
}
