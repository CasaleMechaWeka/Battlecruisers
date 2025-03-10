using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
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
            Logging.Log(Tags.FIRE_INTERVAL_MANAGER, $"{CurrentState}  CurrentState.ShouldFire: {CurrentState.ShouldFire}");
        }

		public void ProcessTimeInterval(float deltaTime)
		{
			CurrentState = CurrentState.ProcessTimeInterval(deltaTime);
            Logging.Verbose(Tags.FIRE_INTERVAL_MANAGER, $"{CurrentState}  deltaTime: {deltaTime}  CurrentState.ShouldFire: {CurrentState.ShouldFire}");
		}
	}
}
