using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public abstract class FireIntervalManagerBase : MonoBehaviour, IFireIntervalManager
	{
		private IState _currentState;
        private IDurationProvider _waitingDurationProvider;

		public virtual void Initialise(IDurationProvider waitingDurationProvider)
		{
            Assert.IsNotNull(waitingDurationProvider);
            _waitingDurationProvider = waitingDurationProvider;

            WaitingState waitingState = new WaitingState();
            IState firingState = CreateFiringState(waitingState);
			waitingState.Initialise(firingState, _waitingDurationProvider);

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
            _waitingDurationProvider.MoveToNextDuration();
		}

		void Update()
		{
			_currentState = _currentState.ProcessTimeInterval(Time.deltaTime);
		}
	}
}
