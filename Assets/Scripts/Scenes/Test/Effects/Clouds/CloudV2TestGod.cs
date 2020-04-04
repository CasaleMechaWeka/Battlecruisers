using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.UI.BattleScene.Clouds;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Effects.Clouds
{
    public class CloudV2TestGod : NavigationTestGod
    {
        public SkyStatsGroup skyStatsGroup;
        public SkyButtonGroup skyButtonGroup;

        protected override void Setup(Helper helper)
        {
            base.Setup(helper);
            BCUtils.Helper.AssertIsNotNull(skyStatsGroup, skyButtonGroup);

            CloudStatsController cloudStatsController = GetComponentInChildren<CloudStatsController>();
            Assert.IsNotNull(cloudStatsController);

            ICloudStats cloudStats 
                = new CloudStats(
                    ConvertMovementSpeed(cloudStatsController.movementSpeed),
                    cloudStatsController.frontCloudColor,
                    cloudStatsController.backCloudColor);

            CloudInitialiser cloudInitialiser = GetComponentInChildren<CloudInitialiser>();
            Assert.IsNotNull(cloudInitialiser);
            cloudInitialiser.Initialise(cloudStats, _updaterProvider.SlowerUpdater);

            skyStatsGroup.Initialise();
            skyButtonGroup.Initialise(skyStatsGroup.SkyStats);
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