using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.UI.BattleScene.Clouds;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Effects.Clouds
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
                    cloudStatsController.legacyDensity,
                    cloudStatsController.movementSpeed);

            CloudInitialiser cloudInitialiser = GetComponentInChildren<CloudInitialiser>();
            Assert.IsNotNull(cloudInitialiser);
            cloudInitialiser.Initialise(cloudStats, BCUtils.RandomGenerator.Instance);
        }
    }
}