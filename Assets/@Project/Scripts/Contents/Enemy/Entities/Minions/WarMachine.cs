public class WarMachine : Entity
{
    protected override void Initialize()
    {
        Controller = new GroundUnitController(this);
        Controller.Initialize();

        StateMachine = new WarMachineStateMachine(this);
    }
}
