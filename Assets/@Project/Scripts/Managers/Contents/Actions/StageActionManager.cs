using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageActionManager
{
    public event Action<int> OnEnemyKilled;
    public event Action<int> OnEnemySpawned;    
    public event Action<float> OnCountDownActive;

    public void CallEnemyKilled(int value) => OnEnemyKilled?.Invoke(value);
    public void CallEnemySpawned(int value) => OnEnemySpawned?.Invoke(value);
    public void CallCountDown(float time) => OnCountDownActive?.Invoke(time);

    public void Clear()
    {
        OnEnemyKilled = null;
        OnEnemySpawned = null;
        OnCountDownActive = null;
    }
}
