using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.UI.BattleScene.Clouds.Teleporters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Fetchers;
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
        public BackgroundImageController background;

        // Reference to the Game 2D Water Kit Material, settable in the Inspector
        public Material waterMaterial; // Manually link this in the Inspector

        public void Initialise(
            string skyMaterialName,
            IUpdater updater,
            float cameraAspectRatio,
            IPrefabContainer<BackgroundImageStats> backgroundStats)
        {
            Helper.AssertIsNotNull(skyMaterialName, updater, moon, fog, skyStatsGroup, background);
            Helper.AssertIsNotNull(leftCloud, rightCloud, mist, backgroundStats);
            Assert.IsTrue(rightCloud.Position.x > leftCloud.Position.x);

            skyStatsGroup.Initialise();
            ISkyStats skyStats = skyStatsGroup.GetSkyStats(skyMaterialName);

            background.Initialise(backgroundStats, cameraAspectRatio, new BackgroundImageCalculator());

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

            // Initialize water material color with SkyStatsController's water color
            if (skyStats is SkyStatsController skyStatsController)
            {
                // Set the color of the water material if assigned in Inspector
                if (waterMaterial != null)
                {
                    // Assuming the Game 2D Water Kit uses "_WaterColor" as the color property name; adjust if different
                    waterMaterial.SetColor("_WaterColor", skyStatsController.WaterColour);
                }
                else
                {
                    Debug.LogError("Water Material is not assigned in CloudInitialiser! Please assign it in the Inspector.");
                }
            }
        }
    }
}
