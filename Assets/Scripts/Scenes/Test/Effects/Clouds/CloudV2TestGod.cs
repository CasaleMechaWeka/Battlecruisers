using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.UI.BattleScene.Clouds;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.Test.Effects.Clouds
{
    public class CloudV2TestGod : NavigationTestGod
    {
        protected override void Setup(Helper helper)
        {
            base.Setup(helper);

            CloudStatsController cloudStatsController = GetComponentInChildren<CloudStatsController>();
            Assert.IsNotNull(cloudStatsController);

            ICloudStats cloudStats 
                = new CloudStats(
                    ConvertMovementSpeed(cloudStatsController.movementSpeed),
                    cloudStatsController.frontCloudColor,
                    cloudStatsController.backCloudColor);

            CloudInitialiserNEW cloudInitialiser = GetComponentInChildren<CloudInitialiserNEW>();
            Assert.IsNotNull(cloudInitialiser);
            cloudInitialiser.Initialise(cloudStats, _updaterProvider.SlowerUpdater);
        }

        private float ConvertMovementSpeed(CloudMovementSpeed movementSpeed)
        {
            switch (movementSpeed)
            {
                case CloudMovementSpeed.Fast:
                    return 0.75f;
                case CloudMovementSpeed.Slow:
                default:
                    return 0.5f;
            }
        }
    }
}