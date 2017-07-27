namespace BattleCruisers.Buildables.Buildings.Turrets.Stats.States
{
    public class BurstFireState : IBurstFireState
    {
        private IBurstFireState _otherState;
        private int _numOfQueriesBeforeSwitch;
        private int _numOfQueries;

        public float DurationInS { get; private set; }

        public bool IsInBurst { get { return _numOfQueries > 0; } }

		public IBurstFireState NextState
        {
            get
            {
                IBurstFireState nextState = this;

                _numOfQueries++;

                if (_numOfQueries >= _numOfQueriesBeforeSwitch)
                {
                    _numOfQueries = 0;
                    nextState = _otherState;
                }

                return nextState;
            }
        }

        // Not constructor because of circular dependency on other state
        public void Initialise(IBurstFireState otherState, float durationInS, int numOfQueriesBeforeSwitch)
        {
            _otherState = otherState;
            DurationInS = durationInS;
            _numOfQueriesBeforeSwitch = numOfQueriesBeforeSwitch;
            _numOfQueries = 0;
        }
    }
}
