using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager
{
    // # Cams
    public event Action<Define.CamType> OnSelectorCam;
    public event Action<Define.CamType> OnUndoMenuCam;

    // # Player
    public event Action OnPlayerDead;

    // # HUD
    public event Action<float> OnCoolDownRepair;
    public event Action<float> OnCoolDownBooster;

    // # LockOn System
    public event Action<Transform> OnLockOnTarget;
    public event Action OnReleaseTarget;
    public event Action<float> OnBossAPChanged;

    // # UI_ArmSelector
    public event Action<UI_ArmSelector.ChangeArmMode> OnArmModeChange;
    public event Action<int> OnArmPartChange;

    // # UI_ShoulderSelector
    public event Action<UI_ShoulderSelector.ChangeShoulderMode> OnShoulderModeChange;
    public event Action<int> OnShoulderPartChange;

    #region CallActions
    public void CallSelectorCam(Define.CamType camType) => OnSelectorCam(camType);
    public void CallUndoMenuCam(Define.CamType camType) => OnUndoMenuCam(camType);

    public void CallPlayerDead() => OnPlayerDead?.Invoke();

    public void CallUseRePair(float percent) => OnCoolDownRepair?.Invoke(percent);
    public void CallUseBooster(float percent) => OnCoolDownBooster?.Invoke(percent);

    public void CallLockOn(Transform target) => OnLockOnTarget?.Invoke(target);
    public void CallRelease() => OnReleaseTarget?.Invoke();
    public void CallBossAPChanged(float percent) => OnBossAPChanged?.Invoke(percent);

    public void CallArmModeChange(UI_ArmSelector.ChangeArmMode armMode) => OnArmModeChange?.Invoke(armMode);
    public void CallArmPartChange(int index) => OnArmPartChange?.Invoke(index);

    public void CallShoulderModeChange(UI_ShoulderSelector.ChangeShoulderMode shoulderMode) => OnShoulderModeChange?.Invoke(shoulderMode);
    public void CallShoulderPartChange(int index) => OnShoulderPartChange?.Invoke(index);
    #endregion
}
