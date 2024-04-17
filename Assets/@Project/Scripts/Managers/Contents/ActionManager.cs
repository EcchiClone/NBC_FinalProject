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
    public event Action<ITarget> OnLockTargetDestroyed;

    // # HUD
    public event Action<float> OnCoolDownRepair;

    // # LockOn System
    public event Action<Transform, float> OnLockOnTarget;
    public event Action OnReleaseTarget;
    public event Action<float> OnTargetAPChanged;

    // # UI_LowerSelector    
    public event Action<int> OnLowerEquip;

    // # UI_UpperSelector    
    public event Action<int> OnUpperEquip;

    // # UI_ArmSelector
    public event Action<UI_ArmSelector.ChangeArmMode> OnArmModeChange;
    public event Action<int> OnArmEquip;

    // # UI_ShoulderSelector
    public event Action<UI_ShoulderSelector.ChangeShoulderMode> OnShoulderModeChange;
    public event Action<int> OnShoulderEquip;

    #region CallActions
    public void CallSelectorCam(Define.CamType camType) => OnSelectorCam(camType);
    public void CallUndoMenuCam(Define.CamType camType) => OnUndoMenuCam(camType);

    public void CallPlayerDead() => OnPlayerDead?.Invoke();
    public void CallLockTargetDestroyed(ITarget target) => OnLockTargetDestroyed?.Invoke(target);

    public void CallUseRePair(float percent) => OnCoolDownRepair?.Invoke(percent);

    public void CallLockOn(Transform target, float percent) => OnLockOnTarget?.Invoke(target, percent);
    public void CallRelease() => OnReleaseTarget?.Invoke();
    public void CallTargetAPChanged(float percent) => OnTargetAPChanged?.Invoke(percent);

    public void CallLowerEquip(int index) => OnLowerEquip?.Invoke(index);
    public void CallUpperEquip(int index) => OnUpperEquip?.Invoke(index);

    public void CallArmModeChange(UI_ArmSelector.ChangeArmMode armMode) => OnArmModeChange?.Invoke(armMode);
    public void CallArmEquip(int index) => OnArmEquip?.Invoke(index);

    public void CallShoulderModeChange(UI_ShoulderSelector.ChangeShoulderMode shoulderMode) => OnShoulderModeChange?.Invoke(shoulderMode);
    public void CallShoulderEquip(int index) => OnShoulderEquip?.Invoke(index);
    #endregion

    public void Clear()
    {
        OnSelectorCam = null;
        OnUndoMenuCam = null;

        OnPlayerDead = null;
        OnLockTargetDestroyed = null;

        OnCoolDownRepair = null;

        OnLockOnTarget = null;
        OnReleaseTarget = null;
        OnTargetAPChanged = null;

        OnLowerEquip = null;
        OnUpperEquip = null;

        OnArmModeChange = null;
        OnArmEquip = null;

        OnShoulderModeChange = null;
        OnShoulderEquip = null;
    }
}
