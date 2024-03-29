using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTiltController
{
    private Module _module;

    private UpperPart _upper;
    private float _smoothValue;

    public WeaponTiltController(Module moudle)
    {
        _module = moudle;

        _upper = _module.CurrentUpper;
        _smoothValue = _module.ModuleStatus.SmoothRotateValue;
    }

    public void CombatFreeFireControl()
    {
        Vector3 headToLookAt = Camera.main.transform.forward;
        Vector3 weaponToLookAt = Camera.main.transform.forward;
        headToLookAt.y = 0;

        LookRotationToTarget(headToLookAt, weaponToLookAt);
    }

    public void CombatLockOnControl()
    {
        Vector3 headToLookAt = _module.LockOnSystem.TargetEnemy.transform.position - _module.transform.position;
        Vector3 weaponToLookAt = _module.LockOnSystem.TargetEnemy.transform.position - _module.transform.position;
        headToLookAt.y = 0;

        LookRotationToTarget(headToLookAt, weaponToLookAt);
    }

    private void LookRotationToTarget(Vector3 headToLookAt, Vector3 weaponToLookAt)
    {
        Quaternion currentHeadRot = _upper.transform.rotation;
        Quaternion currentWeaponTilt = _upper.WeaponTilt.rotation;
        Quaternion targetHeadRotation = Quaternion.LookRotation(headToLookAt);
        Quaternion targetWeaponTilt = Quaternion.LookRotation(weaponToLookAt);
        _upper.transform.rotation = Quaternion.Slerp(currentHeadRot, targetHeadRotation, _smoothValue * Time.deltaTime);
        _upper.WeaponTilt.rotation = Quaternion.Slerp(currentWeaponTilt, targetWeaponTilt, _smoothValue * Time.deltaTime);
    }

    public IEnumerator CoResetRoutine()
    {
        Quaternion currentHead = _upper.transform.localRotation;
        Quaternion currentTilt = _upper.WeaponTilt.localRotation;

        float current = 0f;
        float percent = 0f;
        while (percent < 1f)
        {
            current += Time.deltaTime;
            percent = current / 1f;

            _upper.transform.localRotation = Quaternion.Slerp(currentHead, Quaternion.identity, percent);
            _upper.WeaponTilt.localRotation = Quaternion.Slerp(currentTilt, Quaternion.identity, percent);
            yield return null;
        }

        _upper.transform.localRotation = Quaternion.identity;
        _upper.WeaponTilt.localRotation = Quaternion.identity;
    }
}
