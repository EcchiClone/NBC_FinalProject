using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletOwner
{
    Player,
    Enemy,
}

public class Bullet : MonoBehaviour
{
    [SerializeField] BulletOwner owenr;
}
