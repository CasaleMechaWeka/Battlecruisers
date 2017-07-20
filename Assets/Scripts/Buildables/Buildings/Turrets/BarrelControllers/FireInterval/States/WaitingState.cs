namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public class WaitingState : State
	{
		public override bool ShouldFire { get { return false; } }

		public void Initialise(IState firingState, IFireIntervalProvider fireIntervalProvider)
		{
            base.Initialise(firingState, fireIntervalProvider, initialTimeSinceLastStateChange: float.MaxValue);
		}
	}
}
