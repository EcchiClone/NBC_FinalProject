using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkyFire_States
{
    Chasing,

    Phase1,
    Phase2,

    Dead,
       

    Pattern1_1,
    Pattern1_2,
    Pattern1_3,
    
    Pattern2_1,
    Pattern2_2,
    Pattern2_3,
}

public class SkyFireStateProvider : BossStateProvider
{
    private Dictionary<SkyFire_States, BossBaseState> _states = new Dictionary<SkyFire_States, BossBaseState>();
    // TODO : 추상화 상속 할거면 이것도 그냥 추상 클래스 쪽으로 올려도 될듯

    public SkyFireStateProvider(BossStateMachine context)
        : base(context)
    {
        _states[SkyFire_States.Chasing] = new SkyFire_ChasingState(context, this);
        _states[SkyFire_States.Phase1] = new SkyFire_Phase1State(context, this);
        _states[SkyFire_States.Dead] = new SkyFire_DeadState(context, this);
    }


    // Root
    public override BossBaseState Chasing() => _states[SkyFire_States.Chasing];
    public override BossBaseState Phase1() => _states[SkyFire_States.Phase1];
    public override BossBaseState Dead() => _states[SkyFire_States.Dead];

    //Sub

    //Sub of Sub

}
