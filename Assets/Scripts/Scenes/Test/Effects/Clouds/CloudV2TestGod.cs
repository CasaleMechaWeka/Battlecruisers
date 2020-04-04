using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.UI.BattleScene.Clouds;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Effects.Clouds
{
    public class CloudV2TestGod : TestGodBase
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

            ISkyStats skyStats = skyStatsGroup.GetSkyStats(skybox.material.name);

            CloudInitialiser cloudInitialiser = GetComponentInChildren<CloudInitialiser>();
            Assert.IsNotNull(cloudInitialiser);
            cloudInitialiser.Initialise(skyStats, _updaterProvider.SlowerUpdater);
        }
    }
}