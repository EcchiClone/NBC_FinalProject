using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget
{
    Transform Transform { get; }
    void GetDamaged(float value);
}
