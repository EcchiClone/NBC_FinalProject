using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class WeaponBase : MonoBehaviour
{
    public event Action<int, bool, bool, Define.Parts_Location> OnWeaponFire;

    [SerializeField] protected Define.WeaponType _weaponType;
    [SerializeField] protected GameObject _muzzleEffect;

    public float Damage { get; private set; }
    public float CoolDownTime { get; private set; }
    public float BulletSpeed { get; private set; }
    public float FireRate { get; private set; }
    public float ShotError { get; private set; }
    public int PierceCount { get; private set; }
    public int MaxAmmo { get; private set; }
    public int PerShot {  get; private set; }
    public bool CanReload { get; private set; }

    protected ITarget _target;
    protected LayerMask _groundLayer;
    protected PartData _partData;

    private GameObject _bulletObject;

    private int _ammo;
    private bool _isCoolDown = false;

    public int Ammo
    {
        get => _ammo;
        protected set
        {
            _ammo = value;
            OnWeaponFire?.Invoke(_ammo, _isCoolDown, CanReload, _type);
        }
    }
    public bool IsCoolDown
    {
        get => _isCoolDown;
        protected set
        {
            _isCoolDown = value;
            OnWeaponFire?.Invoke(_ammo, _isCoolDown, CanReload, _type);
        }
    }

    private Define.Parts_Location _type;

    public virtual void Setup(int partID, Define.Parts_Location type, LayerMask layerMask)
    {
        Managers.ActionManager.OnLockOnTarget += Targeting;
        Managers.ActionManager.OnReleaseTarget += Release;

        _groundLayer = layerMask;
        _type = type;
        _partData = Managers.Data.GetPartData(partID);
        _bulletObject = Resources.Load<GameObject>(_partData.BulletPrefab_Path);

        PerkData perkData = Managers.GameManager.PerkData;
        MaxAmmo = (int)(_partData.Ammo * Util.GetIncreasePercentagePerkValue(perkData, PerkType.SpareAmmunition));
        Damage = _partData.Damage * Util.GetIncreasePercentagePerkValue(perkData, PerkType.ImprovedBullet);
        CoolDownTime = _partData.CoolDownTime * Util.GetReducePercentagePerkValue(perkData, PerkType.ImprovedReload);
        BulletSpeed = _partData.BulletSpeed * Util.GetIncreasePercentagePerkValue(perkData, PerkType.RapidFire);
        FireRate = _partData.FireRate * Util.GetReducePercentagePerkValue(perkData, PerkType.OverHeat);
        ShotError = _partData.ShotErrorRange * Util.GetReducePercentagePerkValue(perkData, PerkType.ImprovedBarrel);
        PierceCount = (int)perkData.GetAbilityValue(PerkType.Pierce);
        PerShot = _partData.ProjectilesPerShot;
        CanReload = _partData.IsReloadable;
        if (_type == Define.Parts_Location.Weapon_Shoulder_L || _type == Define.Parts_Location.Weapon_Shoulder_R)
            CanReload = perkData.GetAbilityValue(PerkType.Resupply) == 1f ? true : false;

        Ammo = MaxAmmo;
        if (Managers.Scene.CurrentScene.Scenes == Define.Scenes.TutorialScene)
            Ammo = 9999;

        OnWeaponFire += CheckReload;
    }

    private void Start()
    {
        OnWeaponFire?.Invoke(_ammo, _isCoolDown, CanReload, _type);
    }

    protected GameObject CreateBullet(Transform muzzle)
    {
        GameObject bullet = Util.GetPooler(PoolingType.Player).SpawnFromPool(_bulletObject.name, muzzle.position);
        bullet.transform.rotation = muzzle.rotation;

        return bullet;
    }

    protected EffectLifeTime CreateMuzzleEffect(Transform muzzle)
    {
        EffectLifeTime effect = Util.GetPooler(PoolingType.Player).SpawnFromPool(_muzzleEffect.name, muzzle.position).GetComponent<EffectLifeTime>();
        effect.transform.rotation = muzzle.rotation;

        return effect;
    }

    protected Vector3 GetFreeFireDest()
    {
        Vector3 freeFirePoint = Vector3.up * 100f;
        if (_target == null)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Camera.main.transform.forward, out hit, float.MaxValue, _groundLayer))
                freeFirePoint = hit.point;
        }

        return freeFirePoint;
    }

    public abstract void UseWeapon(Transform[] muzzlePoints);
    private void Targeting(ITarget target, float percent) => _target = target;
    private void Release() => _target = null;

    private void CheckReload(int ammo, bool isCoolDown, bool isReloadable, Define.Parts_Location type)
    {
        if (Ammo <= 0 && isReloadable)
            StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        switch (_weaponType)
        {
            case WeaponType.Arm01:
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Player_Weapon1_Reload, Vector3.zero);
                break;
            case WeaponType.Arm02:
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Player_Weapon2_Reload, Vector3.zero);
                break;
            case WeaponType.Arm03:
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Player_Laser_Reload, Vector3.zero);
                break;
            case WeaponType.Arm04:
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Player_Cannon_Reload, Vector3.zero);
                break;
            case WeaponType.Shoulder01:
               
                break;
            case WeaponType.Shoulder02:
                
                break;
            case WeaponType.Shoulder03:
                
                break;
        }

        yield return Util.GetWaitSeconds(CoolDownTime);

        Ammo = MaxAmmo;
    }
}
