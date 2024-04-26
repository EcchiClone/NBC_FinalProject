public class WarMachine : Entity
{
    protected override void Initialize()
    {
        Controller = new WarMachineController(this);

        StateMachine = new WarMachineStateMachine(this);
    }
}
