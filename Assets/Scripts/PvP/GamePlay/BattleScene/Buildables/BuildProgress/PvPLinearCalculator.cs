using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress
{
    /// <summary>
    /// Calculates build progress linearly.  This is the "normal" build progress,
    /// and how build progress should be calculated in game.
    /// </summary>
    public class PvPLinearCalculator : IPvPBuildProgressCalculator
    {
        private readonly float _buildMultiplier;

        public PvPLinearCalculator(float buildSpeedMultiplier = BuildSpeedMultipliers.DEFAULT)
        {
            // Logging.Log(Tags.BUILD_PROGRESS, $"build speed multiplier: {buildSpeedMultiplier}");

            // should be enabled in Production
            // _buildMultiplier = buildSpeedMultiplier;

            // should be disabled in Production
            _buildMultiplier = BuildSpeedMultipliers.DEFAULT;
        }

        public float CalculateBuildProgressInDroneS(IPvPBuildable buildableUnderConstruction, float deltaTime)
        {
            Assert.IsTrue(buildableUnderConstruction.BuildableState == PvPBuildableState.InProgress);
            // Logging.Log(Tags.BOOST, $"Boost multiplier: {buildableUnderConstruction.BuildProgressBoostable.BoostMultiplier}");

            return
                buildableUnderConstruction.DroneConsumer.NumOfDrones
                * buildableUnderConstruction.BuildProgressBoostable.BoostMultiplier
                * deltaTime
                * _buildMultiplier;
        }
    }
}