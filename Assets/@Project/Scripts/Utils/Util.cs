using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
}