public class Ball : Entity
{
    protected override void Initialize()
    {
        CurrentHelth = Data.maxHealth;

        Controller = new BallUnitController(this);
        Controller.Initialize();

        StateMachine = new BallStateMachine(this);
    }
}
