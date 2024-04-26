using System;
using UnityEngine;

[Serializable]
public class EnemyData : IEntity
{   
    [SerializeField] int dev_ID;
    [SerializeField] string dev_Name;

    [Header("Info")]
    [SerializeField] float stopDistance;
    [SerializeField] float cognizanceRange;
    [SerializeField] float chasingInterval;
    [SerializeField] float rotationSpeed;
    [SerializeField] float fixedAltitude;

    [Header("Status")]
    [SerializeField] float maxHealth;
    [SerializeField] float moveSpeed;
    [SerializeField] float attackInterval;
    [SerializeField] float damage;

    [SerializeField] Define.EnemyType enemyType;    

    public int Dev_ID => dev_ID;
    public string Dev_Name => dev_Name;

    public float StopDistance => stopDistance;
    public float CognizanceRange => cognizanceRange;
    public float ChasingInterval => chasingInterval;
    public float RotationSpeed => rotationSpeed;    
    public float FixedAltitude => fixedAltitude;

    public float MaxHealth => maxHealth;
    public float MoveSpeed => moveSpeed;
    public float AttackInterval => attackInterval;
    public float Damage => damage;
    
    public Define.EnemyType EnemyType => enemyType;    
}
