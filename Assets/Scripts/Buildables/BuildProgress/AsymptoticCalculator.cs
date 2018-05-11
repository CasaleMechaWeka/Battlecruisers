using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.BuildProgress
{
    /// <summary>
    /// Calculates build progress asymptotically, ie, build prgoress never completes.
    /// This is for the tutorial, where we do not want a buildable to complete until
    /// the user has completed some action.
    /// </summary>
    /// FELIX  Update tests
    public class AsymptoticCalculator : IBuildProgressCalculator
    {
        private const float PROPORTION_OF_REMAINING_PROGRESS = 0.05f;
        private const float MAX_BUILD_PROGRESS = 0.95f;

        public float CalculateBuildProgressInDroneS(IBuildable buildableUnderConstruction, float deltaTime)
        {
            Assert.IsTrue(buildableUnderConstruction.BuildableState == BuildableState.InProgress);

            float buildProgressInDroneS = 0;

            if (buildableUnderConstruction.BuildProgress < MAX_BUILD_PROGRESS)
            {
                float buildTimeCompletedInDroneS = buildableUnderConstruction.CostInDroneS * buildableUnderConstruction.BuildProgress;
                float buildTimeRemainingInDroneS = buildableUnderConstruction.CostInDroneS - buildTimeCompletedInDroneS;
                float buildProgressPerS = buildTimeRemainingInDroneS * PROPORTION_OF_REMAINING_PROGRESS;
                buildProgressInDroneS = buildProgressPerS * deltaTime;
            }

            return buildProgressInDroneS;
        }
    }
}