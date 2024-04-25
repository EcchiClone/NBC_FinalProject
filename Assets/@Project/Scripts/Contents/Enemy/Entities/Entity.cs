using UnityEngine;

public abstract class Entity : MonoBehaviour, ITarget
{
    [field: SerializeField] public Define.EnemyType EnemyType { get; set; }

    [field: SerializeField] public EntityDataSO Data { get; set; }
    [field: SerializeField] public Transform Target { get; set; }
    [field: SerializeField] public float Height { get; private set; }
    public float CurrentHelth { get; protected set; }
    [field: SerializeField] public bool IsAlive { get; private set; }// 피격 처리 함수에서만 수정할 수 있도록

    public Controller Controller { get; protected set; }
    public BaseStateMachine StateMachine { get; set; }

    public EnemyBulletPatternStarter enemyPhaseStarter; // TODO : 어디로 가야할지 

    public Animator Anim { get; private set; }

    public Transform Transform => transform;

    public float MaxAP => Data.maxHealth;
    public float AP
    {
        get => CurrentHelth;
        set
        {
            CurrentHelth = value;
            float percent = CurrentHelth / Data.maxHealth;
            if (Managers.Module.CurrentModule.LockOnSystem.TargetEnemy != null && Managers.Module.CurrentModule.LockOnSystem.TargetEnemy.Transform == transform)
                Managers.ActionManager.CallTargetAPChanged(percent);
        }
    }

    public Vector3 Center => transform.position + Vector3.up * Height;

    private void Start()
    {
        enemyPhaseStarter = GetComponent<EnemyBulletPatternStarter>();
        
        Target = Managers.Module.CurrentUpperPart.transform;
        Initialize();

        Anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {        
        StateMachine?.Reset();
        CurrentHelth = Data.maxHealth;
    }

    protected abstract void Initialize();

    public void Activate()
    {
        IsAlive = true;
        AP = Data.maxHealth;
    }

    public void GetDamaged(float damage)
    {
        if (!IsAlive || AP <= 0)
            return;

        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Enemy_Hits, Target.transform.position);
        GameObject go = Util.GetPooler(PoolingType.Enemy).SpawnFromPool("UI_DamagePopup", transform.position);
        UI_DmagePopup damageUI = go.GetComponent<UI_DmagePopup>();
        damageUI.Setup(transform.position, Mathf.RoundToInt(damage));

        AP = Mathf.Max(0, AP - damage);
        if (AP <= 0)
        {
            IsAlive = false;
            Managers.ActionManager.CallLockTargetDestroyed(this);
            if (EnemyType == Define.EnemyType.Minion)
            {
                Managers.StageActionManager.CallMinionKilled();
                AchievementCommonUpdater.instance.GetComponent<UpdateKillMinion>().UpdateReport();
            }
            else if (EnemyType == Define.EnemyType.Boss)
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Boom_Distance, Vector3.zero);
                Managers.StageActionManager.CallBossKilled();
                AchievementCommonUpdater.instance.GetComponent<UpdateKillBoss>().UpdateReport();
            }
        }
    }

    protected virtual void Update()
    {
        Controller?.Update();

        StateMachine?.Update();
    }
}