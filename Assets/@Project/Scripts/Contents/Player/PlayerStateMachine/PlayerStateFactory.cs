using System.Collections.Generic;

public enum States
{
    NonCombat,
    Combat,

    Grounded,
    Jump,
    Fall,
    
    Idle,
    Walk,

    Dash,
}

public class PlayerStateFactory
{
    PlayerStateMachine _context;
    Dictionary<States, PlayerBaseState> _states = new Dictionary<States, PlayerBaseState>();

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;

        _states[States.NonCombat]   = new PlayerNonCombatState(_context, this);
        _states[States.Combat]      = new PlayerCombatState(_context, this);
        _states[States.Grounded]    = new PlayerGroundedState(_context, this);        
        _states[States.Dash]        = new PlayerDashState(_context, this);
        _states[States.Idle]        = new PlayerIdleState(_context, this);
        _states[States.Walk]        = new PlayerWalkState(_context, this);
        _states[States.Jump]        = new PlayerJumpState(_context, this);
        _states[States.Fall]        = new PlayerFallState(_context, this);
    }

    public PlayerBaseState NonCombat() => _states[States.NonCombat];
    public PlayerBaseState Combat() => _states[States.Combat];

    public PlayerBaseState Grounded() => _states[States.Grounded];    
    public PlayerBaseState Jump() => _states[States.Jump];
    public PlayerBaseState Fall() => _states[States.Fall];    
    
    public PlayerBaseState Idle() => _states[States.Idle];
    public PlayerBaseState Walk() => _states[States.Walk];

    public PlayerBaseState Dash() => _states[States.Dash];
}
