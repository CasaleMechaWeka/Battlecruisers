using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.UI.BattleScene.Clouds;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Effects.Clouds
{
    public class CloudV2TestGod : NavigationTestGod
    {
        protected override void Setup(Helper helper)
        {
            base.Setup(helper);

            CloudStatsController cloudStatsController = GetComponentInChildren<CloudStatsController>();
            Assert.IsNotNull(cloudStatsController);
            // -400 <= x <= 400
            float height = cloudStatsController.maxYPosition - cloudStatsController.minYPosition;
            Assert.IsTrue(height > 0);
            Rect cloudSpawnArea = new Rect(x: -400, y: cloudStatsController.minYPosition, width: 800, height: height);
            ICloudGenerationStats cloudStats
                = new CloudGenerationStats(
                    cloudSpawnArea,
                    cloudStatsController.cloudDensityAsFraction,
                    cloudStatsController.movementSpeed,
                    cloudStatsController.frontCloudColor,
                    cloudStatsController.backCloudColor);

            CloudInitialiser cloudInitialiser = GetComponentInChildren<CloudInitialiser>();
            Assert.IsNotNull(cloudInitialiser);
            cloudInitialiser.Initialise(cloudStats, BCUtils.RandomGenerator.Instance);
        }
    }
}