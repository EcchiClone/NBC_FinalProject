
public class Ball_CombatState : BaseState
{
    public Ball_CombatState(BaseStateMachine context, BaseStateProvider provider) : base(context, provider)
    {
        IsRootState = false;
    }
    public override void EnterState()
    {
        
    }

    public override void UpdateState() 
    {
        if(Context.Entity.IsAlive)
            Context.Entity.GetDamaged(Context.Entity.Stat.maxHealth);
    }

    public override void ExitState(){}

    public override void CheckSwitchStates() { }

    public override void InitializeSubState(){}  
}
