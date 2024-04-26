public class Spider : Entity
{
    protected override void Initialize()
    {
        Controller = new GroundUnitController(this);
        Controller.Initialize();

        StateMachine = new SpiderStateMachine(this);
    }
}
