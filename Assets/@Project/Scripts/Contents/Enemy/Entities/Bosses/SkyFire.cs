public class SkyFire : Entity
{  
    protected override void Initialize()
    {
        CurrentHelth = Data.maxHealth;

        Controller = new AirUnitController(this);
        Controller.Initialize();
        StateMachine = new SkyFireStateMachine(this);
    }

}
