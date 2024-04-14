using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDbSheet<T> : ScriptableObject where T : IEntity
{
    public abstract List<T> Entities { get; }
}
