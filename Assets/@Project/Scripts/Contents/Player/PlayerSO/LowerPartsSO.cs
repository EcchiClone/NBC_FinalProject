using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parts/Lower", fileName = "LowerSO_")]
public class LowerPartsSO : BasePartsSO
{
    [Header("# Lower Stats")]
    public float speed;
    public float jumpPower;
    public float boosterPower;
    public bool canJump;
}
