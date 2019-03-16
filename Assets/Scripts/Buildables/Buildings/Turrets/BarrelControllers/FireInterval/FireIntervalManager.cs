using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public class FireIntervalManager : IFireIntervalManager
	{
        private IState _currentState;

		public bool ShouldFire => _currentState.ShouldFire;

		public FireIntervalManager(IState startingState)
		{
            Assert.IsNotNull(startingState);
            _currentState = startingState;
		}

		public void OnFired()
		{
			_currentState = _currentState.OnFired();
		}

		public void ProcessTimeInterval(float deltaTime)
		{
			_currentState = _currentState.ProcessTimeInterval(deltaTime);
		}
	}
}
