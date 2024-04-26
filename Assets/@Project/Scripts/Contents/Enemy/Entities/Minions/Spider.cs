public class Spider : Entity
{
    protected override void Initialize()
    {
        Controller = new GroundUnitController(this);

        StateMachine = new SpiderStateMachine(this);
    }
}
