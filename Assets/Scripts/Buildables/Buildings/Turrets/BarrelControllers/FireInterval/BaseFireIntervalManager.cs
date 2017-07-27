using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public abstract class BaseFireIntervalManager : MonoBehaviour, IFireIntervalManager
	{
		private IState _currentState;

		public virtual void Initialise(IDurationProvider waitingDurationProvider)
		{
            WaitingState waitingState = new WaitingState();
            IState firingState = CreateFiringState(waitingState);
			waitingState.Initialise(firingState, waitingDurationProvider);

			_currentState = firingState;
		}

        protected abstract IState CreateFiringState(IState waitingState);

		public bool ShouldFire()
		{
			return _currentState.ShouldFire;
		}

		public void OnFired()
		{
			_currentState = _currentState.OnFired();
		}

		void Update()
		{
			_currentState = _currentState.ProcessTimeInterval(Time.deltaTime);
		}
	}
}
