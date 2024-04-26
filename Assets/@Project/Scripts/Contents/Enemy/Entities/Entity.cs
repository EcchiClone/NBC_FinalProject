using UnityEngine;

public class EntityStat
{
    [Header("Info")]
    public float stopDistance;
    public float cognizanceRange;
    public float chasingInterval;
    public float rotationSpeed;
    public float fixedAltitude;

    [Header("Status")]
    public float maxHealth;
    public float moveSpeed;
    public float attackInterval;
    public float damage;

    public Define.EnemyType enemyType;

    public EntityStat(EnemyData data)
    {
        stopDistance = data.StopDistance;
        cognizanceRange = data.CognizanceRange;
        chasingInterval = data.ChasingInterval;
        rotationSpeed = data.RotationSpeed;
        fixedAltitude = data.FixedAltitude;

        maxHealth = data.MaxHealth;
        moveSpeed = data.MoveSpeed;
        attackInterval = data.AttackInterval;
        damage = data.Damage;

        enemyType = data.EnemyType;
    }
}

public abstract class Entity : MonoBehaviour, ITarget
{
    static int index = 0;
    public string _tag;
    static int killcount = 0;
    
    [field: SerializeField] public Transform Target { get; set; }
    [field: SerializeField] public float Height { get; private set; }    
    [field: SerializeField] public bool IsAlive { get; private set; }// 피격 처리 함수에서만 수정할 수 있도록
    [field: SerializeField] public Define.EntityType EntityType { get; private set; }

    public EnemyBulletPatternStarter enemyPhaseStarter; // TODO : 어디로 가야할지     
    public BaseStateMachine StateMachine { get; set; }    
    public Controller Controller { get; protected set; }
    public Animator Anim { get; private set; }
    public EntityStat Stat { get; private set; }
    public Define.EnemyType EnemyType { get; private set; }    

    public float FinalAP { get; private set; }
    public float FinalMoveSPD { get; private set; }

    public Transform Transform => transform;
    public Vector3 Center => transform.position + Vector3.up * Height;
    public float MaxAP => FinalAP;
    private float _ap;        
    public float AP
    {
        get => _ap;
        set
        {
            _ap = value;
            float percent = _ap / FinalAP;
            if (Managers.Module.CurrentModule.LockOnSystem.TargetEnemy != null && Managers.Module.CurrentModule.LockOnSystem.TargetEnemy.Transform == transform)
                Managers.ActionManager.CallTargetAPChanged(percent);
        }
    }    

    private bool _isInit = false;

    private void Start()
    {
        _tag = "e" + index++;
        enemyPhaseStarter = GetComponent<EnemyBulletPatternStarter>();
        Anim = GetComponent<Animator>();
        Target = Managers.Module.CurrentUpperPart.transform;

        Initialize();        
    }

    protected abstract void Initialize();

    public void Activate()
    {        
        AP = Stat.maxHealth;          
        IsAlive = true;        
        StateMachine?.Activate();
    }

    public void Setup(LevelData levelData)
    {
        if (!_isInit)
        {
            _isInit = true;
            EnemyData Data = Managers.Data.GetEnemyData((int)EntityType);
            Stat = new EntityStat(Data);
            EnemyType = Data.EnemyType;
        }
        AP = 1; // Ball 자폭 방지
        FinalAP = Stat.maxHealth * levelData.ApModValue;
        FinalMoveSPD = Stat.moveSpeed * levelData.MoveSpdModValue;
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
        GameObject go = Managers.Pool.GetPooler(PoolingType.Enemy).SpawnFromPool("UI_DamagePopup", transform.position);
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