using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class Entity : MonoBehaviour
{
    [field: SerializeField] public EntityDataSO Data { get; set; }
    [field: SerializeField] public Transform Target { get; set; }
    public float CurrentHelth { get; protected set; }
    [field: SerializeField] public bool IsAlive { get; private set; } = true; // 피격 처리 함수에서만 수정할 수 있도록

    public Controller Controller { get; protected set; }
    public BaseStateMachine StateMachine { get; set; }

    public EnemyBulletPatternStarter enemyPhaseStarter; // TODO : 어디로 가야할지 


    private void Start()
    {
        enemyPhaseStarter = GetComponent<EnemyBulletPatternStarter>();
        Target = GameObject.Find("Target").transform;
        //Target = Managers.Module.CurrentModule.transform;
        Initialize();        
    }

    private void OnEnable()
    {
        CurrentHelth = Data.maxHealth;
        StateMachine?.Reset();
    }

    protected abstract void Initialize();


    public void GetDamaged(float damage)
    {
        CurrentHelth = Mathf.Max(0, CurrentHelth - damage);
        //float percent = CurrentHelth / Data.maxHealth;
        //Managers.ActionManager.CallBossAPChanged(percent);
        if(CurrentHelth <= 0)
            IsAlive = false;
    }

    protected virtual void Update()
    {
        Controller?.Update();

        StateMachine?.Update();
    }

    public void StartCoroutine(Action action)
    {
        
    }
}