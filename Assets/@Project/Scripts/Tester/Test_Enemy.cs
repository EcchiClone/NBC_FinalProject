using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Enemy : MonoBehaviour
{
    public float ap;

    public void GetDamage(float damage)
    {
        ap -= damage;
        if (ap <= 0)
            Dead();
    }

    public void Dead()
    {
        gameObject.SetActive(false);
        Managers.ActionManager.CallLockTargetDestroyed(this);
    }
}
