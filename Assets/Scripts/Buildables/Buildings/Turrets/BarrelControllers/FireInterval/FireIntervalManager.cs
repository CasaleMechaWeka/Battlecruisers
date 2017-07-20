using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public class FireIntervalManager : MonoBehaviour, IFireIntervalManager
	{
        private IState _currentState;

        public void Initialise(IFireIntervalProvider waitingDurationProvider, IFireIntervalProvider firingDurationProvider = null)
		{
            if (firingDurationProvider == null)
            {
                firingDurationProvider = new DummyDurationProvider(durationInS: 0);
            }

            FiringState firingState = new FiringState();
            WaitingState waitingState = new WaitingState();

            firingState.Initialise(waitingState, firingDurationProvider);
            waitingState.Initialise(firingState, waitingDurationProvider);

            _currentState = firingState;
		}

		public bool ShouldFire()
		{
            Debug.Log("_currentState: " + _currentState + "  shouldFire: " + _currentState.ShouldFire);

            return _currentState.ShouldFire;
		}

		void Update()
		{
            _currentState = _currentState.ProcessTimeInterval(Time.deltaTime);
		}
	}
}
