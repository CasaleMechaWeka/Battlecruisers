using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test
{
    public class CloudTestGod : NavigationTestGod
    {
        protected override void Setup(Helper helper)
        {
            base.Setup(helper);

            CloudStatsController cloudStatsController = GetComponentInChildren<CloudStatsController>();
            Assert.IsNotNull(cloudStatsController);
            // -110 <= x <= 110
            //  10 <= y <= 60
            Rect cloudSpawnArea = new Rect(x: -110, y: 10, width: 220, height: 50);
            ICloudGenerationStats cloudStats
                = new CloudGenerationStats(
                    cloudSpawnArea,
                    new Range<float>(cloudStatsController.minZPosition, cloudStatsController.maxZPosition),
                    cloudStatsController.density,
                    cloudStatsController.movementSpeed);

            CloudInitialiser cloudInitialiser = GetComponentInChildren<CloudInitialiser>();
            Assert.IsNotNull(cloudInitialiser);
            cloudInitialiser.Initialise(cloudStats, BCUtils.RandomGenerator.Instance);
        }
    }
}