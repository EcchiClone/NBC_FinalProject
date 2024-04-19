public class Turret : Entity
{
    protected override void Initialize()
    {
        CurrentHelth = Data.maxHealth;

        Controller = new TurretUnitController(this);
        Controller.Initialize();

        StateMachine = new TurretStateMachine(this);
    }

}