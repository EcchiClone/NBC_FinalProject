public class Drone : Entity
{
    protected override void Initialize()
    {
        Controller = new AirUnitController(this);
        Controller.Initialize();

        StateMachine = new DroneStateMachine(this);
    }
}
