namespace BattleCruisers.Buildables.Buildings.Turrets.Stats.States
{
    public class BetweenBurstsState : BurstFireState
    {
        public void Initialise(IBurstFireState otherState, float durationInS)
        {
            base.Initialise(otherState, durationInS, numOfQueriesBeforeSwitch: 1, isInBurst: false);
        }
    }
}
