using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDbSheet<T> : ScriptableObject
{
    public abstract List<T> Entities { get; }
}
