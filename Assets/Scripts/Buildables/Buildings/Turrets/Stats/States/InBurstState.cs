namespace BattleCruisers.Buildables.Buildings.Turrets.Stats.States
{
	public class InBurstState : BurstFireState
	{
		public void Initialise(IBurstFireState otherState, float durationInS, int numOfQueriesBeforeSwitch)
		{
			base.Initialise(otherState, durationInS, numOfQueriesBeforeSwitch, isInBurst: true);
		}
	}
}
