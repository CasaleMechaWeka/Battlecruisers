using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    // FELIX  Update tests
    public abstract class DurationState : State
    {
        private float _elapsedTimeInS;
        protected bool _haveFired;

        // No constructor due to circular dependency :)
        public override void Initialise(IState otherState, IDurationProvider durationProvider)
        {
            base.Initialise(otherState, durationProvider);
            Reset();
        }

        public override IState OnFired()
        {
            Logging.VerboseMethod(Tags.FIRE_INTERVAL_MANAGER);

            _durationProvider.MoveToNextDuration();
            _haveFired = true;
            return this;
        }

        public override IState ProcessTimeInterval(float timePassedInS)
        {
            if (!ShouldProcessTimeInterval())
            {
                return this;
            }

            IState nextState = this;

            _elapsedTimeInS += timePassedInS;

            if (_elapsedTimeInS >= _durationProvider.DurationInS)
            {
                Logging.Log(Tags.FIRE_INTERVAL_MANAGER, $"Duration complete:  {this} > {_otherState}");

                Reset();
                nextState = _otherState;
            }

            return nextState;
        }

        private void Reset()
        {
            _elapsedTimeInS = 0;
            _haveFired = false;
        }

        protected virtual bool ShouldProcessTimeInterval() => true;
	}
}
