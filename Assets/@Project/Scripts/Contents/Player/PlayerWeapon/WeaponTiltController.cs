using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTiltController
{
    private Module _module;

    private UpperPart _upper;
    private WeaponPart[] _arms;
    private float _smoothValue;

    public WeaponTiltController(Module moudle)
    {
        _module = moudle;

        _upper = _module.CurrentUpper;
        _arms = new ArmsPart[2];
        _arms[0] = _module.CurrentLeftArm; 
        _arms[1] = _module.CurrentRightArm;

        _smoothValue = _module.ModuleStatus.SmoothRotateValue;
    }

    public void CombatFreeFireControl()
    {
        Vector3 headToLookAt = Camera.main.transform.forward;        
        Vector3 armToLookAt = Camera.main.transform.forward;
        headToLookAt.y = 0;

        LookRotationToTarget(headToLookAt, armToLookAt, armToLookAt);
    }

    public void CombatLockOnControl()
    {
        Vector3 headToLookAt = _module.LockOnSystem.TargetEnemy.Transform.position - _module.transform.position;        
        Vector3 leftArmToLookAt = _module.LockOnSystem.TargetEnemy.Transform.position - _arms[0].transform.position;
        Vector3 rightArmToLookAt = _module.LockOnSystem.TargetEnemy.Transform.position - _arms[1].transform.position;
        headToLookAt.y = 0;

        LookRotationToTarget(headToLookAt, leftArmToLookAt, rightArmToLookAt);
    }

    private void LookRotationToTarget(Vector3 headToLookAt, Vector3 leftArmToLookAt, Vector3 rightArmToLookAt)
    {
        Quaternion currentHeadRot = _upper.transform.rotation;
        Quaternion targetHeadRotation = Quaternion.LookRotation(headToLookAt);       
           
        _upper.transform.rotation = Quaternion.Slerp(currentHeadRot, targetHeadRotation, _smoothValue * Time.deltaTime);        

        Quaternion currentLeftArmRot = _arms[0].transform.rotation;
        Quaternion currentRightArmRot = _arms[1].transform.rotation;
        Quaternion targetLeftArmRotation = Quaternion.LookRotation(leftArmToLookAt);
        Quaternion targetRightArmRotation = Quaternion.LookRotation(rightArmToLookAt);
        _arms[0].transform.rotation = Quaternion.Slerp(currentLeftArmRot, targetLeftArmRotation, _smoothValue * Time.deltaTime);
        _arms[1].transform.rotation = Quaternion.Slerp(currentRightArmRot, targetRightArmRotation, _smoothValue * Time.deltaTime);
    }

    public IEnumerator CoResetRoutine()
    {
        Quaternion currentHead = _upper.transform.localRotation;
        Quaternion currentLeftArmTilt = _arms[0].transform.localRotation;
        Quaternion currentRightArmTilt = _arms[1].transform.localRotation;

        float current = 0f;
        float percent = 0f;
        while (percent < 1f)
        {
            current += Time.deltaTime;
            percent = current / 1f;

            _upper.transform.localRotation = Quaternion.Slerp(currentHead, Quaternion.identity, percent);
            _arms[0].transform.localRotation = Quaternion.Slerp(currentLeftArmTilt, Quaternion.identity, percent);
            _arms[1].transform.localRotation = Quaternion.Slerp(currentRightArmTilt, Quaternion.identity, percent);
            yield return null;
        }

        _upper.transform.localRotation = Quaternion.identity;
        _arms[0].transform.localRotation = Quaternion.identity;
        _arms[1].transform.localRotation = Quaternion.identity;
    }
}
