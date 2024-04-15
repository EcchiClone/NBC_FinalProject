using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public event Action OnInputKeyDown;

    public void CallInputKeyDown()
    {
        if (Input.anyKeyDown)
            OnInputKeyDown?.Invoke();
    }

    public void Clear()
    {
        OnInputKeyDown = null;
    }
}
