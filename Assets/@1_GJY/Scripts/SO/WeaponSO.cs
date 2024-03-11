using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Parts/Weapon", fileName = "Weapon_")]
public class WeaponSO : ScriptableObject
{
    [Header("Info")]
    public int id;
    public string dev_Name;

    public string displayName;
    public string description;

    [Header("Stats")]
    public GameObject bulletPrefab;

    [Range(0.01f, 2f)] public float fireRate;
    [Range(1f, 5f)] public float coolDownTime;
    [Range(1, 20)] public float projectilesPerShot;    
}
