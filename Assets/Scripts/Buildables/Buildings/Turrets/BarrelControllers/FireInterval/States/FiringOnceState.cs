namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public class FiringOnceState : State
    {
        public override bool ShouldFire => true;

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