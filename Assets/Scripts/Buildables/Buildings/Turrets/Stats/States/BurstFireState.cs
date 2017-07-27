using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.Stats.States
{
    public abstract class BurstFireState : IBurstFireState
    {
        private IBurstFireState _otherState;
        private int _numOfQueriesBeforeSwitch;
        private int _numOfQueries;

        public float DurationInS { get; private set; }

        public bool IsInBurst { get; private set; }

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
        protected void Initialise(IBurstFireState otherState, float durationInS, int numOfQueriesBeforeSwitch, bool isInBurst)
        {
            _otherState = otherState;
            DurationInS = durationInS;
            _numOfQueriesBeforeSwitch = numOfQueriesBeforeSwitch;
            IsInBurst = isInBurst;
            _numOfQueries = 0;
        }
    }
}
