public class SkyFire : Entity
{  
    protected override void Initialize()
    {
        Controller = new AirUnitController(this);
        StateMachine = new SkyFireStateMachine(this);
    }

}
