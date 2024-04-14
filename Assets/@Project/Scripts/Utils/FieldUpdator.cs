using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FieldUpdator
{
    public static void UpdateValueAndNotify(ref int field, int value, Action notifyAction)
    {
        field = value;
        notifyAction?.Invoke();
    }

    public static void UpdateValueAndNotify(ref int field, int value, Action<int> notifyAction)
    {
        field = value;
        notifyAction?.Invoke(value);
    }

    public static void UpdateValueAndNotify(ref float field, float value, Action notifyAction)
    {
        field = value;
        notifyAction?.Invoke();
    }    

    public static void UpdateValueAndNotify(ref float field, float value, Action<float> notifyAction)
    {
        field = value;
        notifyAction?.Invoke(value);
    }
}
