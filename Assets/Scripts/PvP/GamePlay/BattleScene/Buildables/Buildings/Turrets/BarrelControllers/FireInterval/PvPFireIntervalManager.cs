using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval.States;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public class PvPFireIntervalManager : IPvPFireIntervalManager
    {
        private IPvPState _currentState;
        private IPvPState CurrentState
        {
            get => _currentState;
            set
            {
                Assert.IsNotNull(value);
                _currentState = value;
                _shouldFire.Value = _currentState.ShouldFire;
            }
        }

        private readonly PvPSettableBroadcastingProperty<bool> _shouldFire;
        public IPvPBroadcastingProperty<bool> ShouldFire { get; }

        public PvPFireIntervalManager(IPvPState startingState)
        {
            Assert.IsNotNull(startingState);

            _currentState = startingState;
            _shouldFire = new PvPSettableBroadcastingProperty<bool>(_currentState.ShouldFire);
            ShouldFire = new PvPBroadcastingProperty<bool>(_shouldFire);
        }

        public void OnFired()
        {
            CurrentState = CurrentState.OnFired();
            // Logging.Log(Tags.FIRE_INTERVAL_MANAGER, $"{CurrentState}  CurrentState.ShouldFire: {CurrentState.ShouldFire}");
        }

        public void ProcessTimeInterval(float deltaTime)
        {
            CurrentState = CurrentState.ProcessTimeInterval(deltaTime);
            // Logging.Verbose(Tags.FIRE_INTERVAL_MANAGER, $"{CurrentState}  deltaTime: {deltaTime}  CurrentState.ShouldFire: {CurrentState.ShouldFire}");
        }
    }
}
