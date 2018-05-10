using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.BuildProgress
{
    // FELIX  Test :)
    public class LinearCalculator : IBuildProgressCalculator
    {
        private readonly float _buildMultiplier;

        private const float DEFAULT_BUILD_MULTIPLIER = 1;

        public LinearCalculator(float buildMultiplier = DEFAULT_BUILD_MULTIPLIER)
        {
            _buildMultiplier = buildMultiplier;
        }

        public float CalculateBuildProgressInDroneS(IBuildable buildableUnderConstruction, float deltaTime)
        {
            Assert.IsTrue(buildableUnderConstruction.BuildableState == BuildableState.InProgress);

            return 
                buildableUnderConstruction.DroneConsumer.NumOfDrones 
                * buildableUnderConstruction.BuildProgressBoostable.BoostMultiplier 
                * deltaTime 
                * _buildMultiplier;
        }
    }
}