public class WarMachine : Entity
{
    protected override void Initialize()
    {
        CurrentHelth = Data.maxHealth;

        Controller = new GroundUnitController(this);
        Controller.Initialize();

        StateMachine = new WarMachineStateMachine(this);
    }
}
