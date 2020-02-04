using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using UnityCommon.Properties;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public class FireIntervalManager : IFireIntervalManager
	{
        private IState _currentState;
        private IState CurrentState
        {
            get => _currentState;
            set
            {
                Assert.IsNotNull(value);
                _currentState = value;
                _shouldFire.Value = _currentState.ShouldFire;
            }
        }

        private readonly SettableBroadcastingProperty<bool> _shouldFire;
        public IBroadcastingProperty<bool> ShouldFire { get; }

		public FireIntervalManager(IState startingState)
		{
            Assert.IsNotNull(startingState);

            _currentState = startingState;
            _shouldFire = new SettableBroadcastingProperty<bool>(_currentState.ShouldFire);
            ShouldFire = new BroadcastingProperty<bool>(_shouldFire);
		}

        public void OnFired()
		{
			CurrentState = CurrentState.OnFired();
		}

		public void ProcessTimeInterval(float deltaTime)
		{
			CurrentState = CurrentState.ProcessTimeInterval(deltaTime);
		}
	}
}
