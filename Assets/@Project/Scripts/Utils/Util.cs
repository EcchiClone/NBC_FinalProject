using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Util
{
    private static Dictionary<float, WaitForSeconds> _waitDict = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWaitSeconds(float seconds)
    {
        if (_waitDict.TryGetValue(seconds, out var wait) == false)
        {
            _waitDict.Add(seconds, new WaitForSeconds(seconds));
            return _waitDict[seconds];
        }
        return wait;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);

        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (!recursive)
        {
            //To Do - UI 이외의 작업을 위한 resurcive false 조건일 때 로직

            for (int i = 0; i < go.transform.childCount; ++i)
            {
                Transform transform = go.transform.GetChild(i);
                if (transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static Quaternion RandomDirectionFromMuzzle(float shotErrorRange)
    {
        float xError = SetShotErrorRange(shotErrorRange);
        float yError = SetShotErrorRange(shotErrorRange);
        Quaternion rotation = Quaternion.Euler(yError, xError, 0f); // 각도 계산

        return rotation;
    }

    public static float SetShotErrorRange(float standard)
    {
        float x1 = Random.Range(0f, 1f);
        float x2 = Random.Range(0f, 1f);

        return standard * (Mathf.Sqrt(-2.0f * Mathf.Log(x1)) * Mathf.Sin(2.0f * Mathf.PI * x2));
    }

    public static float GetCurrentAnimationClipLength(Animator animator)
    {
        AnimatorStateInfo clipInfo = animator.GetCurrentAnimatorStateInfo(0);
        float length = clipInfo.length;

        return length;
    }

    public static float CalcPerkValueToStatus(PerkType type, float origin, float adjust)
    {
        float resultValue = 0;

        switch (type)
        {
            case PerkType.SuperAllow:
                resultValue = 0;
                break;
            case PerkType.SpeedModifier:
                break;
            case PerkType.BoosterOverload:
                break;
            case PerkType.AfterBurner:
                break;
            case PerkType.ImprovedReload:
                break;
            case PerkType.RapidFire:
                break;
            case PerkType.Spring:
                break;
            case PerkType.Lubrication:
                break;
            case PerkType.Jetpack:
                break;
            case PerkType.Pierce:
                break;
            case PerkType.ImprovedBullet:
                break;
            case PerkType.Rador:
                break;
            case PerkType.OverHeat:
                break;
            case PerkType.ImprovedBarrel:
                break;
            case PerkType.SpareAmmunition:
                break;
            case PerkType.Resupply:
                break;
            case PerkType.ImprovedArmor:
                break;
            case PerkType.Stealth:
                break;
        }
        return resultValue;
    }
}