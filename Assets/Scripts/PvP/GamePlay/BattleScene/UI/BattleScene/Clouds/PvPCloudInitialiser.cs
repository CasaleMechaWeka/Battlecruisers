using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Teleporters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds
{
    public class PvPCloudInitialiser : MonoBehaviour
    {
        private PvPCloudTeleporter _cloudTeleporter;

        public PvPCloudController leftCloud, rightCloud;
        public PvPMistController mist;
        public PvPMoonController moon;
        public PvPFogController fog;
        public PvPSkyStatsGroup skyStatsGroup;
        public PvPBackgroundImageController background;

        // References to SeaBackground, UnderwaterGlow, and SeaShade
        public SpriteRenderer seaBackgroundSprite;
        public SpriteRenderer underwaterGlowSprite;
        public Image seaShadeCanvas;

        public void Initialise(
            string skyMaterialName,
            IPvPUpdater updater,
            float cameraAspectRatio,
            IPvPPrefabContainer<PvPBackgroundImageStats> backgroundStats)
        {
            PvPHelper.AssertIsNotNull(skyMaterialName, updater, moon, fog, skyStatsGroup, background);
            PvPHelper.AssertIsNotNull(leftCloud, rightCloud, mist, backgroundStats);
            Assert.IsTrue(rightCloud.Position.x > leftCloud.Position.x);

            skyStatsGroup.Initialise();
            IPvPSkyStats skyStats = skyStatsGroup.GetSkyStats(skyMaterialName);

            background.Initialise(backgroundStats, cameraAspectRatio, new PvPBackgroundImageCalculator());

            leftCloud.Initialise(skyStats);
            rightCloud.Initialise(skyStats);

            IPvPCloudRandomiser cloudRandomiser
                = new PvPCloudRandomiser(
                    PvPRandomGenerator.Instance,
                    rightCloudValidXPositions: new PvPRange<float>(min: -100, max: 400));
            cloudRandomiser.RandomiseStartingPosition(leftCloud, rightCloud);

            _cloudTeleporter
                = new PvPCloudTeleporter(
                    updater,
                    new PvPTeleporterHelper(),
                    leftCloud,
                    rightCloud);

            mist.Initialise(skyStats);
            moon.Initialise(skyStats.MoonStats);
            fog.Initialise(skyStats.FogColour);

            // Apply WaterColour to SeaBackground sprite, UnderwaterGlow sprite, and SeaShade canvas
            if (skyStats is PvPSkyStatsController pvPSkyStatsController)
            {
                ApplyWaterColourToElements(pvPSkyStatsController.WaterColour);
            }
        }

        private void ApplyWaterColourToElements(Color waterColour)
        {
            // Set the SeaBackground sprite to WaterColour
            if (seaBackgroundSprite != null)
            {
                seaBackgroundSprite.color = waterColour;
            }
            else
            {
                Debug.LogError("SeaBackground SpriteRenderer is not assigned! Please assign it in the Inspector.");
            }

            // Set the UnderwaterGlow sprite to WaterColour
            if (underwaterGlowSprite != null)
            {
                underwaterGlowSprite.color = waterColour;
            }
            else
            {
                Debug.LogError("UnderwaterGlow SpriteRenderer is not assigned! Please assign it in the Inspector.");
            }

            // Set the SeaShade canvas to WaterColour
            if (seaShadeCanvas != null)
            {
                seaShadeCanvas.color = waterColour;
            }
            else
            {
                Debug.LogError("SeaShade Canvas Image is not assigned! Please assign it in the Inspector.");
            }
        }
    }
}
