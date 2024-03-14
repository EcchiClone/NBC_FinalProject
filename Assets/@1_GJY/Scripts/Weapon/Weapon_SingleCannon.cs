using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_SingleCannon : Weapon_Primary
{
    public override void UseWeapon_Primary(Transform[] muzzlePoints)
    {
        foreach (Transform muzzle in muzzlePoints)
        {
            float xError = SetShotErrorRange();
            float yError = SetShotErrorRange();

            GameObject bullet = Instantiate(WeaponSO.bulletPrefab);
            bullet.transform.position = muzzle.position;
            bullet.transform.rotation = muzzle.rotation;

            Quaternion rotation = Quaternion.Euler(yError, xError, 0f); // 각도 계산

            bullet.transform.rotation *= rotation; // 현재 방향에 추가 회전을 적용
        }
    }

    private float SetShotErrorRange(float standard = 0.5f)
    {
        float x1 = Random.Range(0f, 1f);
        float x2 = Random.Range(0f, 1f);

        return standard * (Mathf.Sqrt(-2.0f * Mathf.Log(x1)) * Mathf.Sin(2.0f * Mathf.PI * x2));
    }
}
