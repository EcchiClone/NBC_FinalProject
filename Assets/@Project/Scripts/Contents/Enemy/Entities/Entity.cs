using System;
using System.Collections.Generic;
using UnityEngine;


public enum Pattern
{
    Phase1_Pattern1,
    Phase1_Pattern2,
    Phase1_Pattern3,
    
    Phase2_Pattern1,
    Phase2_Pattern2,
    Phase2_Pattern3,
}

public abstract class Entity : MonoBehaviour
{
    [field: SerializeField] public EntityDataSO Data { get; set; }
    [field: SerializeField] public Transform Target { get; set; }
    public float CurrentHelth { get; protected set; }
    public bool IsAlive { get; private set; } = true; // 피격 처리 함수에서만 수정할 수 있도록
    public Controller Controller { get; protected set; }
    public BaseStateMachine StateMachine { get; set; }

    public Dictionary<Pattern, Action<string>> Patterns { get; protected set; } = new Dictionary<Pattern, Action<string>>();

    public EnemyPhaseStarter enemyPhaseStarter; // TODO : 어디로 가야할지 

    private void Start()
    {
        enemyPhaseStarter = GetComponent<EnemyPhaseStarter>();
        Initialize();        
    }

    protected abstract void Initialize();

    public void GetDamaged(float damage)
    {
        CurrentHelth = Mathf.Max(0, CurrentHelth - damage);
        float percent = CurrentHelth / Data.maxHealth;
        Managers.ActionManager.CallBossAPChanged(percent);
        if(CurrentHelth <= 0)
            IsAlive = false;
    }
}