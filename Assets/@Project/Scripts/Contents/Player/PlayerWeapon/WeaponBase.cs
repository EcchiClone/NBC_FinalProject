using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected Transform _target;    
    protected Transform _weaponTransform { get; set; }
    protected LayerMask _groundLayer;    

    protected PartData _partData;

    protected bool _isCoolDown = false;

    public virtual void Setup(int partID, Transform bodyTransform, LayerMask layerMask) 
    {
        Managers.ActionManager.OnLockOnTarget += Targeting;
        Managers.ActionManager.OnReleaseTarget += Release;

        _weaponTransform = bodyTransform;
        _groundLayer = layerMask;
        _partData = Managers.Data.GetPartData(partID);
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
            if (Physics.Raycast(_weaponTransform.position, Camera.main.transform.forward, out hit, float.MaxValue, _groundLayer))
                freeFirePoint = hit.point;
        }

        return freeFirePoint;
    }

    public abstract void UseWeapon(Transform[] muzzlePoints);
    private void Targeting(Transform target) => _target = target;
    private void Release() => _target = null;
}
