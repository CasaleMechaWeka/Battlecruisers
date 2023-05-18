using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories.Spawning
{
    /// <summary>
    /// If the unit under construction is destroyed, do not want to immediately
    /// start buliding the next unit.  This avoids the factory being "protected"
    /// by instantly respawning in progress units.  Ignore if the unit under 
    /// construction was recently changed.
    /// </summary>
    public class PvPCooldownSpawnDecider : IPvPUnitSpawnDecider
    {
        private readonly IPvPUnitSpawnTimer _unitSpawnTimer;

        public const float MIN_BUILD_BREAK_IN_S = 0.5f;

        public PvPCooldownSpawnDecider(IPvPUnitSpawnTimer unitSpawnTimer)
        {
            Assert.IsNotNull(unitSpawnTimer);
            _unitSpawnTimer = unitSpawnTimer;
        }

        public bool CanSpawnUnit(IPvPUnit unitToSpawn)
        {
            // Logging.Verbose(Tags.SPAWN_DECIDER,
            //     $"Time since chosen: {_unitSpawnTimer.TimeSinceUnitWasChosenInS}  " +
            //     $"Time since clear:  {_unitSpawnTimer.TimeSinceFactoryWasClearInS}");

            if (_unitSpawnTimer.TimeSinceUnitWasChosenInS >= MIN_BUILD_BREAK_IN_S
                && _unitSpawnTimer.TimeSinceFactoryWasClearInS <= MIN_BUILD_BREAK_IN_S)
            {
                // Logging.Verbose(Tags.SPAWN_DECIDER, "Times mean false, cannot spawn unit  :(!");
                return false;
            }
            else
            {
                // Logging.Verbose(Tags.SPAWN_DECIDER, "Times mean true, can spawn unit!  :)");
                return true;
            }
        }
    }
}