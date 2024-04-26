using UnityEngine;

public abstract class Entity : MonoBehaviour, ITarget
{
    static int index = 0;
    public string _tag;
    static int killcount = 0;
    [field: SerializeField] public Define.EnemyType EnemyType { get; set; }

    [field: SerializeField] public EntityDataSO Data { get; set; }
    [field: SerializeField] public Transform Target { get; set; }
    [field: SerializeField] public float Height { get; private set; }
    public float CurrentHelth { get; protected set; }
    public bool IsInit { get; private set; } = false;
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
        _tag = "e" + index++;
        enemyPhaseStarter = GetComponent<EnemyBulletPatternStarter>();
        
        Target = Managers.Module.CurrentUpperPart.transform;
        Initialize();

        Anim = GetComponent<Animator>();
        Debug.LogError($"tag : {_tag} IsInit : {IsInit}");
    }

    private void OnEnable()
    {
        IsInit = false;
        Debug.LogError($"tag : {_tag} OnEnable");
        
    }

    private void OnDisable()
    {
        IsInit = false;
    }

    protected abstract void Initialize();

    public void Activate()
    {
        IsInit = true;
        Debug.LogError($"tag : {_tag} Activate");
        IsAlive = true;
        AP = Data.maxHealth;
        StateMachine?.Activate();
    }

    public void GetDamaged(float damage)
    {
        if(!IsInit)
        {
            return;
        }
        Debug.LogError($"1 tag : {_tag} isAlive: {IsAlive} ap : {AP} Active : {gameObject.activeSelf} IsInit : {IsInit}");

        if (!IsAlive || AP <= 0)
            return;

        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Enemy_Hits, Target.transform.position);
        GameObject go = Util.GetPooler(PoolingType.Enemy).SpawnFromPool("UI_DamagePopup", transform.position);
        UI_DmagePopup damageUI = go.GetComponent<UI_DmagePopup>();
        damageUI.Setup(transform.position, Mathf.RoundToInt(damage));

        AP = Mathf.Max(0, AP - damage);
        IsAlive = AP > 0;

        if (!IsAlive)
        {
            if(this is Ball)
            {
                ++killcount;
                Debug.Log(killcount);
                Debug.LogError($"2 tag : {_tag} isAlive: {IsAlive} ap : {AP} Active : {gameObject.activeSelf}");
            }
            

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