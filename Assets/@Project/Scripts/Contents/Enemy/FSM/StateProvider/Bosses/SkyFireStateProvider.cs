using System.Collections.Generic;


public class SkyFireStateProvider : BaseStateProvider
{
    private Dictionary<Boss_States, BaseState> _states = new Dictionary<Boss_States, BaseState>();
    // TODO : 추상화 상속 할거면 이것도 그냥 추상 클래스 쪽으로 올려도 될듯

    public SkyFireStateProvider(BaseStateMachine context)
        : base(context)
    {
        _states[Boss_States.Alive] = new SkyFire_AliveState(context, this);
        _states[Boss_States.Dead] = new SkyFire_DeadState(context, this);

        _states[Boss_States.Chasing] = new SkyFire_ChasingState(context, this);
        _states[Boss_States.Phase1] = new SkyFire_Phase1State(context, this);
        _states[Boss_States.Phase2] = new SkyFire_Phase2State(context, this);
    }


    // Root
    public BaseState Alive() => _states[Boss_States.Alive];
    public BaseState Dead() => _states[Boss_States.Dead];
    // Sub
    public BaseState Chasing() => _states[Boss_States.Chasing];
    public BaseState Phase1() => _states[Boss_States.Phase1];
    public BaseState Phase2() => _states[Boss_States.Phase2];

    //Sub of Sub

}
