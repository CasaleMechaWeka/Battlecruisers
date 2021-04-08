using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.BuildProgress
{
    /// <summary>
    /// Calculates build progress linearly.  This is the "normal" build progress,
    /// and how build progress should be calculated in game.
    /// </summary>
    public class LinearCalculator : IBuildProgressCalculator
    {
        private readonly float _buildMultiplier;

        public LinearCalculator(float buildSpeedMultiplier = BuildSpeedMultipliers.DEFAULT)
        {
            Logging.Log(Tags.BUILD_PROGRESS, $"build speed multiplier: {buildSpeedMultiplier}");
            _buildMultiplier = buildSpeedMultiplier;
        }

        public float CalculateBuildProgressInDroneS(IBuildable buildableUnderConstruction, float deltaTime)
        {
            Assert.IsTrue(buildableUnderConstruction.BuildableState == BuildableState.InProgress);
            Logging.Log(Tags.BOOST, $"Boost multiplier: {buildableUnderConstruction.BuildProgressBoostable.BoostMultiplier}");

            return 
                buildableUnderConstruction.DroneConsumer.NumOfDrones 
                * buildableUnderConstruction.BuildProgressBoostable.BoostMultiplier 
                * deltaTime 
                * _buildMultiplier;
        }
    }
}