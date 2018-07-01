namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    // FELIX  Test!!! + all other states & fire interval manager :/
    public class FiringOnceState : IState
    {
        private IState _waitingState;

        public bool ShouldFire { get { return true; } }

		// No constructor due to circular dependency :)
		public void Initialise(IState waitingState)
        {
            _waitingState = waitingState;
        }

        public IState OnFired()
        {
            return _waitingState;
        }

        public IState ProcessTimeInterval(float timePassedInS)
        {
            // Do nothing
            return this;
        }
    }
}