using UnityEngine;

public interface ITarget
{
    Transform Transform { get; }
    Vector3 Center { get; }
    Define.EnemyType EnemyType { get; }
    bool IsAlive { get; }
    float MaxAP { get; }
    float AP { get; }
    
    void GetDamaged(float value);
}
