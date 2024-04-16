using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public event Action OnInputKeyDown;

    public void CallInputKeyDown()
    {
        if (Input.anyKeyDown || Input.GetMouseButton(0) || Input.GetMouseButton(1))
            OnInputKeyDown?.Invoke();
    }

    public void Clear()
    {
        OnInputKeyDown = null;
    }
}
