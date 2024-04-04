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
    public float chasingInterval;
    public float rotationSpeed;

    [Header("Status")]
    public float maxHealth;
    public float moveSpeed;
}
