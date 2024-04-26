public class Drone : Entity
{
    protected override void Initialize()
    {
        Controller = new AirUnitController(this);

        StateMachine = new DroneStateMachine(this);
    }
}
