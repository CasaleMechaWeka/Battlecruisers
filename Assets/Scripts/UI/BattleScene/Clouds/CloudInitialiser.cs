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
        public FogController fog;
        public SkyStatsGroup skyStatsGroup;
        public BackgroundStatsList backgroundStatsList;
        public BackgroundImageController background;

        public void Initialise(string skyMaterialName, IUpdater updater, int levelNum, float cameraAspectRatio)
        {
            Helper.AssertIsNotNull(skyMaterialName, updater, moon, fog, skyStatsGroup, backgroundStatsList, background);
            Helper.AssertIsNotNull(leftCloud, rightCloud, mist);
            Assert.IsTrue(rightCloud.Position.x > leftCloud.Position.x);

            skyStatsGroup.Initialise();
            ISkyStats skyStats = skyStatsGroup.GetSkyStats(skyMaterialName);

            backgroundStatsList.Initialise();
            IBackgroundImageStats backgroudStats = backgroundStatsList.GetStats(levelNum);
            background.Initialise(backgroudStats, cameraAspectRatio, new BackgroundImageCalculator());

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

            mist.Initialise(skyStats);
            moon.Initialise(skyStats.MoonStats);
            fog.Initialise(skyStats.FogColour);
        }

        /// <summary>
        /// For a level we only need the stats once, to set the sky and the background.
        /// Afterwards, they can be safely destroyed.
        /// 
        /// For test scenes, we want to keep the stats, as we want to by able to change
        /// the sky/backrgound.
        /// </summary>
        public void DestroyStats()
        {
            Destroy(skyStatsGroup.gameObject);
            Destroy(backgroundStatsList.gameObject);
        }
    }
}