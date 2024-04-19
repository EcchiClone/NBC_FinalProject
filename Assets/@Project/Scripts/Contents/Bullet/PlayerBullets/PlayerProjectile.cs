using System.Collections;
using UnityEngine;

public class PlayerProjectile : Bullet
{
    [SerializeField] GameObject _hitEffectPrefab;
    [SerializeField] TrailRenderer _trailRenderers;
    [SerializeField] protected Define.BulletType _bulletType;
    [SerializeField] protected Define.BulletHitSounds _sfxType;
    [SerializeField] protected LayerMask _damagableLayer;

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

    public virtual void Setup(float speed, float damage, bool splash, Vector3 groundTargetPos, Transform target = null, float explosiveRange = 0)
    {
        _speed = speed;
        _damage = damage;
        _isSplash = splash;
        _explosiveRange = explosiveRange;
        if (_trailRenderers != null)
            _trailRenderers.Clear();

        _coroutine = StartCoroutine(Co_LifeTime());
    }

    private IEnumerator Co_LifeTime()
    {
        yield return Util.GetWaitSeconds(LIFE_TIME);

        CreateEffect();
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((_damagableLayer & (1 << collision.gameObject.layer)) != 0)
        {
            StopAllCoroutines();
            CreateEffect();
            PlaySFX();

            if (_isSplash)
            {
                RaycastHit[] hits = Physics.SphereCastAll(transform.position, _explosiveRange, Vector3.up, 0, _damagableLayer);

                foreach (var hit in hits) // 범위에 들어간 적은 데미지 부여
                {
                    if (hit.transform.TryGetComponent(out ITarget entity))
                        entity.GetDamaged(_damage);
                    else if (hit.transform.TryGetComponent(out DummyController dummy))
                        dummy.GetDamaged(_damage, transform.position);
                }
            }
            else
            {
                if (collision.gameObject.TryGetComponent(out ITarget entity))
                    entity.GetDamaged(_damage);
                else if (collision.gameObject.TryGetComponent(out DummyController dummy))
                    dummy.GetDamaged(_damage, transform.position);
            }            
            gameObject.SetActive(false);
        }
    }

    private void PlaySFX()
    {
        // 사용하는 것만 추가
        // enum에 커서대면 어떤 순서인지 나와용
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
        GameObject go = ObjectPooler.SpawnFromPool(_hitEffectPrefab.name, transform.position);
        EffectLifeTime hitEffect = go.GetComponent<EffectLifeTime>();
        hitEffect.Setup();
        hitEffect.transform.rotation = transform.rotation;
    }

    private void OnDisable()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
        ObjectPooler.ReturnToPool(gameObject); // 한 객체에 한번만        
        CancelInvoke();
    }
}
