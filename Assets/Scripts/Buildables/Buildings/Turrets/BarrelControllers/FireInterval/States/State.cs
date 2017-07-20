namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
	public abstract class State : IState
	{
        private IState _otherState;
		private IFireIntervalProvider _fireIntervalProvider;
		private float _timeElapsedInS;
		private float _timeSinceLastStateChangeInS;

        public abstract bool ShouldFire { get; }

        // No constructor due to circular dependency :)
        public void Initialise(IState otherState, IFireIntervalProvider fireIntervalProvider, float initialTimeSinceLastStateChange)
		{
			_otherState = otherState;
			_fireIntervalProvider = fireIntervalProvider;
            _timeSinceLastStateChangeInS = initialTimeSinceLastStateChange;
		}

		public IState ProcessTimeInterval(float timePassedInS)
		{
			IState nextState = this;

			if (_timeSinceLastStateChangeInS >= _timeElapsedInS)
			{
				_timeSinceLastStateChangeInS = 0;
				_timeElapsedInS = _fireIntervalProvider.NextFireIntervalInS;
				nextState = _otherState;
			}

			return nextState;
		}
	}
}
