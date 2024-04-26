public class Turret : Entity
{
    protected override void Initialize()
    {
        Controller = new TurretUnitController(this);
        Controller.Initialize();

        StateMachine = new TurretStateMachine(this);
    }

}