using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Enemy : MonoBehaviour, ITarget
{
    public float maxAP;
    public float currentAP;
    public bool isAlive;

    private void Awake()
    {
        isAlive = true;
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

    public bool IsAlive => isAlive;

    public Define.EnemyType EnemyType { get; set; } = Define.EnemyType.Tutorial;

    public Vector3 Center => transform.position += Vector3.up * 0.5f;

    public void GetDamaged(float damage)
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Enemy_Hits, transform.position);

        if (!Managers.Tutorial.IsMissioning)
            return;

        AP -= damage;
        if (AP <= 0)
            Dead();
    }

    private void LockOn(ITarget transform, float t)
    {
        if (transform.Transform == this.transform)
            Managers.AchievementSystem.ReceiveReport("TUTO8", "Locked", 1);
    }

    public void Dead()
    {
        isAlive = false;
        gameObject.SetActive(false);
        Managers.ActionManager.CallLockTargetDestroyed(this);
        Managers.AchievementSystem.ReceiveReport("TUTO9", "Killed", 1);
    }
}
