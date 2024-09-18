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

        // References to Fancy Water and Fancy Water Surface Materials
        public Material fancyWaterMaterial;  // Background water material (solid color)
        public Material fancyWaterSurfaceMaterial;  // Foreground water material (gradient)

        // Reference to the OpaqueDepthsMask sprite renderer
        public SpriteRenderer opaqueDepthsMask;

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

            // Apply WaterColour to both fancyWaterMaterial (solid) and fancyWaterSurfaceMaterial (gradient) materials
            if (skyStats is SkyStatsController skyStatsController)
            {
                ApplyWaterColourToMaterials(skyStatsController.WaterColour);
            }
        }

        private void ApplyWaterColourToMaterials(Color waterColour)
        {
            // Set fancyWaterMaterial (background) material to the solid WaterColour
            if (fancyWaterMaterial != null)
            {
                fancyWaterMaterial.SetColor("_WaterColor", waterColour);  // Assuming _WaterColor is the correct property
            }
            else
            {
                Debug.LogError("Fancy Water Material is not assigned! Please assign it in the Inspector.");
            }

            // Apply a gradient to fancyWaterSurfaceMaterial (foreground)
            if (fancyWaterSurfaceMaterial != null)
            {
                // Create a gradient from WaterColour (0% opacity at the top) to WaterColour (100% opacity at the bottom)
                Color opaqueColor = new Color(waterColour.r, waterColour.g, waterColour.b, 1f);
                Color transparentColor = new Color(waterColour.r, waterColour.g, waterColour.b, 0f);

                // Set the gradient start and end colors using the existing properties in the shader
                fancyWaterSurfaceMaterial.SetColor("_WaterColorGradientStart", transparentColor);
                fancyWaterSurfaceMaterial.SetColor("_WaterColorGradientEnd", opaqueColor);

                // Enable gradient in the shader
                fancyWaterSurfaceMaterial.SetFloat("_Water2D_IsColorGradientEnabled", 1f);
            }
            else
            {
                Debug.LogError("Fancy Water Surface Material is not assigned! Please assign it in the Inspector.");
            }

            // Set the OpaqueDepthsMask sprite to the WaterColour with 100% opacity
            if (opaqueDepthsMask != null)
            {
                Color fullyOpaqueColor = new Color(waterColour.r, waterColour.g, waterColour.b, 1f);
                opaqueDepthsMask.color = fullyOpaqueColor;
            }
            else
            {
                Debug.LogError("OpaqueDepthsMask SpriteRenderer is not assigned! Please assign it in the Inspector.");
            }
        }

    }
}
