public class Spider : Entity
{
    protected override void Initialize()
    {
        CurrentHelth = Data.maxHealth;

        Controller = new GroundUnitController(this);
        Controller.Initialize();

        StateMachine = new SpiderStateMachine(this);
    }
}
