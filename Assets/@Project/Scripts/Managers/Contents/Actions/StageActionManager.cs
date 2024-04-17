using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageActionManager
{    
    public event Action OnMinionKilled;
    public event Action OnBossKilled;

    public event Action<int> OnEnemySpawned;    
    public event Action<float> OnCountDownActive;

    public event Action OnResult;

    public void CallMinionKilled() => OnMinionKilled?.Invoke();
    public void CallBossKilled() => OnBossKilled?.Invoke();

    public void CallEnemySpawned(int value) => OnEnemySpawned?.Invoke(value);
    public void CallCountDown(float time) => OnCountDownActive?.Invoke(time);

    public void CallResulet() => OnResult?.Invoke();

    public void Clear()
    {
        OnMinionKilled = null;
        OnBossKilled = null;           
        OnEnemySpawned = null;
        OnCountDownActive = null;
        OnResult = null;
    }
}
