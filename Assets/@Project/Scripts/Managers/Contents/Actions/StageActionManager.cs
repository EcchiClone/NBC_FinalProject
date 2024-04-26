using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageActionManager
{
    public event Action OnBossStage;

    public event Action OnMinionKilled;
    public event Action OnBossKilled;

    public event Action<int> OnEnemySpawned;
    public event Action<float> OnCountDownActive;

    public event Action<StageData> OnResult;

    public void CallBossStage() => OnBossStage?.Invoke();

    public void CallMinionKilled() => OnMinionKilled?.Invoke();
    public void CallBossKilled() => OnBossKilled?.Invoke();

    public void CallEnemySpawned(int value) => OnEnemySpawned?.Invoke(value);
    public void CallCountDown(float time) => OnCountDownActive?.Invoke(time);

    public void CallResult(StageData data) => OnResult?.Invoke(data);

    public void Clear()
    {
        OnBossStage = null;
        OnMinionKilled = null;
        OnBossKilled = null;           
        OnEnemySpawned = null;
        OnCountDownActive = null;
        OnResult = null;
    }
}
