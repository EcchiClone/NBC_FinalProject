using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager
{
    // # Player
    public event Action OnPlayerDead;

    // # HUD
    public event Action<float> OnCoolDownRepair;
    public event Action<float> OnCoolDownBooster;

    // # LockOn System
    public event Action<Transform> OnLockOnTarget;
    public event Action OnReleaseTarget;
    public event Action<float> OnBossAPChanged;

    public void CallPlayerDead() => OnPlayerDead?.Invoke();

    public void CallUseRePair(float percent) => OnCoolDownRepair?.Invoke(percent);
    public void CallUseBooster(float percent) => OnCoolDownBooster?.Invoke(percent);

    public void CallLockOn(Transform target) => OnLockOnTarget?.Invoke(target);
    public void CallRelease() => OnReleaseTarget?.Invoke();
    public void CallBossAPChanged(float percent) => OnBossAPChanged?.Invoke(percent);
}
