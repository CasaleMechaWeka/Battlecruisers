using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public class LaserFireIntervalManagerInitialiser : FireIntervalManagerBase
	{
        private IDurationProvider _firingDurationProvider;

		public void Initialise(IDurationProvider waitingDurationProvider, IDurationProvider firingDurationProvider)
		{
            _firingDurationProvider = firingDurationProvider;

            base.Initialise(waitingDurationProvider);
		}

        protected override IState CreateFiringState(IState waitingState)
        {
            FiringDurationState firingState = new FiringDurationState();
            firingState.Initialise(waitingState, _firingDurationProvider);
            return firingState;
        }
    }
}
