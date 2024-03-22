using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossType
{
    GroundBoss,
    FlyingBoss,
}

[Serializable]
[CreateAssetMenu(menuName = "Boss/Data", fileName = "BossData")]
public class BossDataSO : ScriptableObject
{
    [Header("Info")]
    public BossType BossType;
    public float stopDistance;
    public float chasingInterval;
    public float rotationSpeed;

    [Header("Status")]
    public float maxHealth;
    public float moveSpeed;
}
