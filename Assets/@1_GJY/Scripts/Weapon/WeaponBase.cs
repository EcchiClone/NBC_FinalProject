using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [field: SerializeField] public WeaponSO WeaponSO { get; private set; }
    [SerializeField] private LayerMask _groundLayer;
    
    protected Transform _target;

    private void Awake()
    {
        Managers.ActionManager.OnLockOnTarget += Targeting;
        Managers.ActionManager.OnReleaseTarget += Release;
    }

    public virtual void Setup() {}

    protected void RandomDirectionShot(Transform[] muzzlePoints)
    {
        foreach (Transform muzzle in muzzlePoints)
        {
            Vector3 freeFirePoint = Vector3.up * 100f;
            if (_target == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Camera.main.transform.forward, out hit, float.MaxValue, _groundLayer))
                    freeFirePoint = hit.point;
            }
            
            GameObject go = EnemyBulletPoolManager.instance.GetGo(WeaponSO.bulletName);
            go.transform.position = muzzle.position;
            go.transform.rotation = muzzle.rotation;

            float xError = SetShotErrorRange(WeaponSO.shotErrorRange);
            float yError = SetShotErrorRange(WeaponSO.shotErrorRange);
            Quaternion rotation = Quaternion.Euler(yError, xError, 0f); // 각도 계산
            go.transform.rotation *= rotation; // 현재 방향에 추가 회전을 적용

            PlayerProjectile projectile = go.GetComponent<PlayerProjectile>();
            projectile.Setup(WeaponSO.speed, freeFirePoint, _target);
        }
    }

    protected float SetShotErrorRange(float standard)
    {
        float x1 = Random.Range(0f, 1f);
        float x2 = Random.Range(0f, 1f);

        return standard * (Mathf.Sqrt(-2.0f * Mathf.Log(x1)) * Mathf.Sin(2.0f * Mathf.PI * x2));
    }

    private void Targeting(Transform target) => _target = target;
    private void Release() => _target = null;
}
