using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States
{
    public abstract class PvPDurationState : PvPState
    {
        private float _elapsedTimeInS;
        protected bool _haveFired;

        // No constructor due to circular dependency :)
        public override void Initialise(IPvPState otherState, IPvPDurationProvider durationProvider)
        {
            base.Initialise(otherState, durationProvider);
            Reset();
        }

        public override IPvPState OnFired()
        {
            // Logging.VerboseMethod(Tags.FIRE_INTERVAL_MANAGER);

            _durationProvider.MoveToNextDuration();
            _haveFired = true;
            return this;
        }

        public override IPvPState ProcessTimeInterval(float timePassedInS)
        {
            if (!ShouldProcessTimeInterval())
            {
                return this;
            }

            IPvPState nextState = this;

            _elapsedTimeInS += timePassedInS;

            if (_elapsedTimeInS >= _durationProvider.DurationInS)
            {
                // Logging.Log(Tags.FIRE_INTERVAL_MANAGER, $"Duration complete:  {this} > {_otherState}");

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
