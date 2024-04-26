public class WarMachine : Entity
{
    protected override void Initialize()
    {
        Controller = new GroundUnitController(this);

        StateMachine = new WarMachineStateMachine(this);
    }
}
