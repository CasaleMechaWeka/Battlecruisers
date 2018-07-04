using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public class FireIntervalManager : IFireIntervalManager
	{
        private readonly IDurationProvider _waitingDurationProvider;
        private IState _currentState;

		public FireIntervalManager(IState startingState, IDurationProvider waitingDurationProvider)
		{
            Helper.AssertIsNotNull(startingState, waitingDurationProvider);

            _waitingDurationProvider = waitingDurationProvider;
            _currentState = startingState;
		}

		public bool ShouldFire()
		{
			return _currentState.ShouldFire;
		}

		public void OnFired()
		{
			_currentState = _currentState.OnFired();
            _waitingDurationProvider.MoveToNextDuration();
		}

		public void Update(float deltaTime)
		{
			_currentState = _currentState.ProcessTimeInterval(deltaTime);
		}
	}
}
