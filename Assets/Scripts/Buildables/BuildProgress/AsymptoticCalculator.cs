using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.BuildProgress
{
    /// <summary>
    /// Calculates build progress asymptotically, ie, build prgoress never completes.
    /// This is for the tutorial, where we do not want a buildable to complete until
    /// the user has completed some action.
    /// </summary>
    public class AsymptoticCalculator : IBuildProgressCalculator
    {
        private const float PROPORTION_OF_REMAINING_PROGRESS = 0.2f;

        public float CalculateBuildProgressInDroneS(IBuildable buildableUnderConstruction, float deltaTime)
        {
            Assert.IsTrue(buildableUnderConstruction.BuildableState == BuildableState.InProgress);

            float totalBuildTimeInDroneS = buildableUnderConstruction.BuildTimeInS * buildableUnderConstruction.CostInDroneS;
            float buildTimeCompletedInDroneS = totalBuildTimeInDroneS * buildableUnderConstruction.BuildProgress;
            float buildTimeRemainingInDroneS = totalBuildTimeInDroneS - buildTimeCompletedInDroneS;
            float buildProgressPerS = buildTimeRemainingInDroneS * PROPORTION_OF_REMAINING_PROGRESS;
            return buildProgressPerS * deltaTime;
        }
    }
}