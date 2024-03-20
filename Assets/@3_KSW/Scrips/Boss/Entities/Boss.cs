using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    [field: SerializeField] public BossDataSO Data { get; set; }
    [field: SerializeField] public Transform Target{ get; set; } 


    public float CurrentHelth { get; protected set; }
    public bool IsAlive { get; private set; } // 피격 처리 함수에서만 수정할 수 있도록


    public BossController Controller { get; protected set; }
    public BossStateMachine StateMachine { get; set; }


    private void Start()
    {
        Initialize();
    }

    protected abstract void Initialize();

    public void GetDamaged(float damage)
    {
        CurrentHelth = Mathf.Max(0, CurrentHelth - damage);
        if(CurrentHelth <= 0)
            IsAlive = false;
    }
}