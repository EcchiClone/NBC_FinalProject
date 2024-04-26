public class SkyFire : Entity
{  
    protected override void Initialize()
    {
        Controller = new AirUnitController(this);
        Controller.Initialize();
        StateMachine = new SkyFireStateMachine(this);
    }

}
