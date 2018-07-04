namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public abstract class DurationState : State
	{
		private float _elapsedTimeInS;

        // No constructor due to circular dependency :)
        public override void Initialise(IState otherState, IDurationProvider durationProvider)
		{
            base.Initialise(otherState, durationProvider);

            _elapsedTimeInS = 0;
		}

        public override IState OnFired()
        {
            _durationProvider.MoveToNextDuration();
            return this;
        }

        public override IState ProcessTimeInterval(float timePassedInS)
		{
			IState nextState = this;

            _elapsedTimeInS += timePassedInS;

			if (_elapsedTimeInS >= _durationProvider.DurationInS)
			{
			    _elapsedTimeInS = 0;
                nextState = _otherState;
            }

            return nextState;
        }
	}
}
