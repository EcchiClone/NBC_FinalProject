public class Ball : Entity
{
    protected override void Initialize()
    {
        Controller = new BallUnitController(this);

        StateMachine = new BallStateMachine(this);
    }
}
