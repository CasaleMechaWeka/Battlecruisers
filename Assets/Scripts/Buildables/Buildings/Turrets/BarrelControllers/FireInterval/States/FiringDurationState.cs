namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public class FiringDurationState : DurationState
    {
        public override bool ShouldFire => true;

        /// <summary>
        /// Do not start processing (counting down) firing interval until the first firing has occurred.
        /// Otherwise laser firing duration decreases even when no laser has been fired (Archen)!
        /// </summary>
        protected override bool ShouldProcessTimeInterval()
        {
            return _haveFired;
        }
    }
}
