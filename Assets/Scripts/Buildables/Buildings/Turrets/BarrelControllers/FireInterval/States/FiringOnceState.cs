namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    // FELIX  Test!!! + all other states & fire interval manager :/
    public class FiringOnceState : State
    {
        public override bool ShouldFire { get { return true; } }

        public override IState OnFired()
        {
            _durationProvider.MoveToNextDuration();
            return _otherState;
        }

        public override IState ProcessTimeInterval(float timePassedInS)
        {
            // Do nothing
            return this;
        }
    }
}