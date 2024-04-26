using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Util
{
    private static Dictionary<float, WaitForSeconds> WaitDict = new Dictionary<float, WaitForSeconds>();
    private static Dictionary<PoolingType, ObjectPooler> PoolDict = new Dictionary<PoolingType, ObjectPooler>();

    public static WaitForSeconds GetWaitSeconds(float seconds)
    {
        if (WaitDict.TryGetValue(seconds, out var wait) == false)
        {
            WaitDict.Add(seconds, new WaitForSeconds(seconds));
            return WaitDict[seconds];
        }
        return wait;
    }

    public static void SetPooler(ObjectPooler pooler) => PoolDict.Add(pooler.PoolingType, pooler);
    public static ObjectPooler GetPooler(PoolingType type) => PoolDict[type];
    public static void ClearPooler()
    {
        IsCleared = true;
        PoolDict.Clear();
    }
    public static bool IsCleared { get; set; }

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

    public static float GetIncreasePercentagePerkValue(PerkData data, PerkType type) => 1f + data.GetAbilityValue(type) / 100f;
    public static float GetReducePercentagePerkValue(PerkData data, PerkType type) => 1f - data.GetAbilityValue(type) / 100f;
}