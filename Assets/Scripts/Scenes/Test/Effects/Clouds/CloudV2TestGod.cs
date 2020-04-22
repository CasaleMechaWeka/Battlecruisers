using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.UI.BattleScene.Clouds;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Effects.Clouds
{
    public class CloudV2TestGod : NavigationTestGod
    {
        public Skybox skybox;
        public SkyStatsGroup skyStatsGroup;
        public SkyButtonGroup skyButtonGroup;

        protected override void Setup(Helper helper)
        {
            base.Setup(helper);
            BCUtils.Helper.AssertIsNotNull(skybox, skyStatsGroup, skyButtonGroup);

            skyStatsGroup.Initialise();
            skyButtonGroup.Initialise(skyStatsGroup.SkyStats);

            CloudInitialiser cloudInitialiser = GetComponentInChildren<CloudInitialiser>();
            Assert.IsNotNull(cloudInitialiser);
            cloudInitialiser.Initialise(skybox.material.name, _updaterProvider.SlowerUpdater);
        }
    }
}