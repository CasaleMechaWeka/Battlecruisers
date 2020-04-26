using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.UI.BattleScene.Clouds.Teleporters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public class CloudInitialiser : MonoBehaviour
    {
        private CloudTeleporter _cloudTeleporter;

        public CloudController leftCloud, rightCloud;
        public MistController mist;
        public MoonController moon;

        public void Initialise(string skyMaterialName, IUpdater updater)
        {
            Helper.AssertIsNotNull(skyMaterialName, updater, moon);
            Helper.AssertIsNotNull(leftCloud, rightCloud, mist);
            Assert.IsTrue(rightCloud.Position.x > leftCloud.Position.x);

            SkyStatsGroup skyStatsGroup = GetComponentInChildren<SkyStatsGroup>();
            Assert.IsNotNull(skyStatsGroup);
            skyStatsGroup.Initialise();
            ISkyStats skyStats = skyStatsGroup.GetSkyStats(skyMaterialName);

            leftCloud.Initialise(skyStats);
            rightCloud.Initialise(skyStats);

            ICloudRandomiser cloudRandomiser
                = new CloudRandomiser(
                    RandomGenerator.Instance,
                    rightCloudValidXPositions: new Range<float>(min: -100, max: 400));
            cloudRandomiser.RandomiseStartingPosition(leftCloud, rightCloud);

            _cloudTeleporter
                = new CloudTeleporter(
                    updater,
                    new TeleporterHelper(),
                    leftCloud,
                    rightCloud);

            // FELIX  TEMP
            //mist.Initialse(skyStats.MistColour);
            moon.Initialise(skyStats.MoonStats);
        }
    }
}