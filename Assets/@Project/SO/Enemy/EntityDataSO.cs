using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
[CreateAssetMenu(menuName = "Entity/Data", fileName = "EntityData")]
public class EntityDataSO : ScriptableObject
{
    [Header("Info")]
    public float stopDistance;
    public float cognizanceRange;
    public float chasingInterval;
    public float rotationSpeed;
    public float patrolDistance;
    public float fixedAltitude;

    [Header("Status")]
    public float maxHealth;
    public float moveSpeed;
    public float attackInterval;
}
