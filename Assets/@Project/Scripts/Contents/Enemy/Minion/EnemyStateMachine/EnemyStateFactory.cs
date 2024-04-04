public class EnemyStateFactory
{
    EnemyStateMachine _context;

    public EnemyStateFactory(EnemyStateMachine currentContext)
    {
        _context = currentContext;
    }

    public EnemyBaseState Grounded() => new EnemyGroundedState(_context, this);
    public EnemyBaseState Chasing() => new EnemyChasingState(_context, this);
    public EnemyBaseState Standoff() => new EnemyStandoffState(_context, this);
    public EnemyBaseState Run() => new EnemyRunState(_context, this);
}
