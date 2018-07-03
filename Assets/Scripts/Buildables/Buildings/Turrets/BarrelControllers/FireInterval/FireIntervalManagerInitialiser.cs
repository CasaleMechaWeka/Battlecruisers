using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public class FireIntervalManagerInitialiser : BaseFireIntervalManager
	{
        public override void Initialise(IDurationProvider waitingDurationProvider)
		{
            base.Initialise(waitingDurationProvider);
		}

        protected override IState CreateFiringState(IState waitingState)
        {
            FiringOnceState firingState = new FiringOnceState();
            firingState.Initialise(waitingState);
            return firingState;
        }
   	}
}
