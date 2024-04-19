public class Drone : Entity
{
    protected override void Initialize()
    {
        CurrentHelth = Data.maxHealth;

        Controller = new AirUnitController(this);
        Controller.Initialize();

        StateMachine = new DroneStateMachine(this);
    }
}
