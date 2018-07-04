using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    // FELIX  Avoid duplicate code between states, base class?
    // FELIX  Test!!! + all other states & fire interval manager :/
    public class FiringOnceState : IState
    {
        private IState _waitingState;
        private IDurationProvider _waitingDurationProvider;

        public bool ShouldFire { get { return true; } }

		// No constructor due to circular dependency :)
		public void Initialise(IState waitingState, IDurationProvider waitingDurationProvider)
        {
            Helper.AssertIsNotNull(waitingState, waitingDurationProvider);

            _waitingState = waitingState;
            _waitingDurationProvider = waitingDurationProvider;
        }

        public IState OnFired()
        {
            _waitingDurationProvider.MoveToNextDuration();
            return _waitingState;
        }

        public IState ProcessTimeInterval(float timePassedInS)
        {
            // Do nothing
            return this;
        }
    }
}