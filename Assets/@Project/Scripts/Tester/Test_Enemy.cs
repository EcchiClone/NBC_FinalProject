using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Enemy : MonoBehaviour, ITarget
{
    public float maxAP;
    public float currentAP;

    private void Awake()
    {
        Managers.ActionManager.OnLockOnTarget += LockOn;
    }

    public Transform Transform => transform;

    public float MaxAP => maxAP;
    public float AP
    {
        get => currentAP;
        set
        {
            currentAP = value;
            float percent = currentAP / maxAP;
            Managers.ActionManager.CallTargetAPChanged(percent);
        }
    }    

    public void GetDamaged(float damage)
    {
        if (!Managers.Tutorial.IsMissioning)
            return;

        AP -= damage;
        if (AP <= 0)
            Dead();
    }

    private void LockOn(Transform transform, float t)
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
