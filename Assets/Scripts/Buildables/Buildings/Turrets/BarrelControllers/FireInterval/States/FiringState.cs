namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public class FiringState : State
    {
        public override bool ShouldFire { get { return true; } }

        public void Initialise(IState waitingState, IFireIntervalProvider fireIntervalProvider)
        {
            base.Initialise(waitingState, fireIntervalProvider, initialTimeSinceLastStateChange: 0);
        }
    }
}
