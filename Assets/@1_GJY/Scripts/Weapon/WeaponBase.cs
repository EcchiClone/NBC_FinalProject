using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [field: SerializeField] public WeaponSO WeaponSO {  get; private set; }

    public virtual void Setup() { }

    protected void RandomDirectionShot(Transform[] muzzlePoints)
    {
        foreach (Transform muzzle in muzzlePoints)
        {
            float xError = SetShotErrorRange(WeaponSO.shotErrorRange);
            float yError = SetShotErrorRange(WeaponSO.shotErrorRange);

            GameObject bullet = Instantiate(WeaponSO.bulletPrefab, muzzle.position, muzzle.rotation);

            Quaternion rotation = Quaternion.Euler(yError, xError, 0f); // 각도 계산
            bullet.transform.rotation *= rotation; // 현재 방향에 추가 회전을 적용
        }        
    }

    protected float SetShotErrorRange(float standard)
    {
        float x1 = Random.Range(0f, 1f);
        float x2 = Random.Range(0f, 1f);

        return standard * (Mathf.Sqrt(-2.0f * Mathf.Log(x1)) * Mathf.Sin(2.0f * Mathf.PI * x2));
    }
}
