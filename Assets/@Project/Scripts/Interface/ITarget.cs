using UnityEngine;

public interface ITarget
{
    bool IsAlive { get; }
    float MaxAP { get; }
    float AP { get; }
    Transform Transform { get; }
    void GetDamaged(float value);
}
