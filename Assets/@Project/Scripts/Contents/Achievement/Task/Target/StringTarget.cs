using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Achievement/Task/Target/String", fileName = "Target_String_")]
public class StringTarget : TaskTarget
{
    [SerializeField]
    private string value;
    public override object Value => value;

    public override bool IsEqual(object target)
    {
        string targetAsString = target as string; // target을 string형으로 캐스팅
        if (targetAsString == null)
            return false;
        return value == targetAsString;
    }
}