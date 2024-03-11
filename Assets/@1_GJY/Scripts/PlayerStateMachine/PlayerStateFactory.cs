public class PlayerStateFactory
{
    PlayerStateMachine _context;

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
    }

    public PlayerBaseState Idle() => new PlayerIdleState(_context, this);
    public PlayerBaseState Walk() => new PlayerWalkState(_context, this);
    public PlayerBaseState Jump() => new PlayerJumpState(_context, this);
    public PlayerBaseState Grounded() => new PlayerGroundedState(_context, this);
    public PlayerBaseState Fall() => new PlayerFallState(_context, this);
    public PlayerBaseState Combat() => new PlayerCombatState(_context, this);
}
