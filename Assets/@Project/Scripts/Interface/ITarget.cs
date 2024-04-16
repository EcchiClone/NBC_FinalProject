using UnityEngine;

public interface ITarget
{
    float MaxAP { get; }
    float AP { get; }
    Transform Transform { get; }
    void GetDamaged(float value);
}
