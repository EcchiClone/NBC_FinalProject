public class Ball : Entity
{
    protected override void Initialize()
    {
        Controller = new BallUnitController(this);
        Controller.Initialize();

        StateMachine = new BallStateMachine(this);
    }
}
