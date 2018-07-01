namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public abstract class DurationState : IState
	{
        private IState _otherState;
		private IDurationProvider _durationProvider;
		private float _elapsedTimeInS;

        public abstract bool ShouldFire { get; }

        // No constructor due to circular dependency :)
        public void Initialise(IState otherState, IDurationProvider durationProvider)
		{
			_otherState = otherState;
            _durationProvider = durationProvider;

			_elapsedTimeInS = 0;
		}

        public IState OnFired()
        {
            // Do nothing
            return this;
        }

        public IState ProcessTimeInterval(float timePassedInS)
		{
			IState nextState = this;

            _elapsedTimeInS += timePassedInS;

			if (_elapsedTimeInS >= _durationProvider.DurationInS)
			{
                ResetTime();

                nextState = _otherState;
            }

            return nextState;
        }

        private void ResetTime()
        {
            // FELIX
			_elapsedTimeInS = 0;
            //_durationProvider.MoveToNextDuration();
        }
	}
}
