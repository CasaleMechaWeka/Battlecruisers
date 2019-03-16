namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public class WaitingState : DurationState
	{
		public override bool ShouldFire => false;
	}
}
