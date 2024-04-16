using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Enemy : MonoBehaviour, ITarget
{
    public float ap;

    private void Awake()
    {
        Managers.ActionManager.OnLockOnTarget += LockOn;
    }

    public Transform Transform => transform;

    public void GetDamaged(float damage)
    {
        ap -= damage;
        if (ap <= 0)
            Dead();
    }

    private void LockOn(Transform transform)
    {
        if (transform == this.transform)
            Managers.AchievementSystem.ReceiveReport("TUTO8", "Locked", 1);
    }

    public void Dead()
    {        
        gameObject.SetActive(false);
        Managers.ActionManager.CallLockTargetDestroyed(this);
        Managers.AchievementSystem.ReceiveReport("TUTO9", "Killed", 1);
    }
}
