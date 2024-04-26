public class Turret : Entity
{
    protected override void Initialize()
    {
        Controller = new TurretUnitController(this);

        StateMachine = new TurretStateMachine(this);
    }

}