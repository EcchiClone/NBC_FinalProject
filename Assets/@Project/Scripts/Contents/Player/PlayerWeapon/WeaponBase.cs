using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected Transform _target;
    protected LayerMask _groundLayer;

    protected PartData _partData;
    protected bool _isCoolDown = false;

    private Define.PartsType _type;
    private int _ammo;
    public int Ammo
    {
        get => _ammo;
        protected set
        {
            _ammo = value;
            OnWeaponFire?.Invoke(_ammo, _partData.IsReloadable, _type);
        }
    }

    public event Action<int, bool, Define.PartsType> OnWeaponFire;

    public virtual void Setup(int partID, Define.PartsType type, LayerMask layerMask)
    {
        Managers.ActionManager.OnLockOnTarget += Targeting;
        Managers.ActionManager.OnReleaseTarget += Release;

        _groundLayer = layerMask;
        _type = type;
        _partData = Managers.Data.GetPartData(partID);
        Ammo = _partData.Ammo;

        OnWeaponFire += CheckReload;
    }

    private void Start()
    {
        OnWeaponFire?.Invoke(_ammo, _partData.IsReloadable, _type);
    }

    protected GameObject CreateBullet(Transform muzzle)
    {
        GameObject go = Resources.Load<GameObject>(_partData.BulletPrefab_Path);
        GameObject bullet = Instantiate(go);
        bullet.transform.position = muzzle.position;
        bullet.transform.rotation = muzzle.rotation;

        return bullet;
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
    private void Targeting(Transform target) => _target = target;
    private void Release() => _target = null;

    private void CheckReload(int ammo, bool isReloadable, Define.PartsType type)
    {
        if (Ammo <= 0 && isReloadable)
            StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        yield return Util.GetWaitSeconds(_partData.CoolDownTime);

        Ammo = _partData.Ammo;
    }
}
