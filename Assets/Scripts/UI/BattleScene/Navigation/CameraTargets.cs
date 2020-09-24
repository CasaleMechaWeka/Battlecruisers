using BattleCruisers.Cruisers;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class CameraTargets : ICameraTargets
    {
        public ICameraTarget PlayerCruiserTarget { get; }
        public ICameraTarget PlayerCruiserDeathTarget { get; }
        public ICameraTarget PlayerCruiserNukedTarget { get; }
        public ICameraTarget PlayerNavalFactoryTarget { get; }

        public ICameraTarget AICruiserTarget { get; }
        public ICameraTarget AICruiserDeathTarget { get; }
        public ICameraTarget AICruiserNukedTarget { get; }
        public ICameraTarget AINavalFactoryTarget { get; }

        public ICameraTarget MidLeftTarget { get; }
        public ICameraTarget OverviewTarget { get; }

        private const float CRUISER_DEATH_ORTHOGRAPHIC_SIZE = 10;
        private const float MID_ORTHOGRAPHIC_SIZE = 15;
        private const float NUKE_ORTHOGRAPHIC_SIZE = 30;

        public CameraTargets(
            ICameraCalculator cameraCalculator,
            ICameraCalculatorSettings cameraCalculatorSettings,
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            ICamera camera)
        {
            Helper.AssertIsNotNull(cameraCalculator, cameraCalculatorSettings, playerCruiser, aiCruiser, camera);

            PlayerCruiserTarget = FindCruiserTarget(camera, cameraCalculator, playerCruiser);
            AICruiserTarget = FindCruiserTarget(camera, cameraCalculator, aiCruiser);

            // Overview
            Vector3 overviewPosition = camera.Position;
            overviewPosition.y = cameraCalculator.FindCameraYPosition(cameraCalculatorSettings.ValidOrthographicSizes.Max);
            OverviewTarget = new CameraTarget(overviewPosition, cameraCalculatorSettings.ValidOrthographicSizes.Max);

            IRange<float> midXPositions = cameraCalculator.FindValidCameraXPositions(MID_ORTHOGRAPHIC_SIZE);
            MidLeftTarget = CreateTarget(camera, cameraCalculator, MID_ORTHOGRAPHIC_SIZE, midXPositions.Min);

            // Player cruiser naval factory
            float playerCruiserBowSlotXPosition = playerCruiser.Position.x + playerCruiser.Size.x / 2;
            PlayerNavalFactoryTarget = CreateTarget(camera, cameraCalculator, cameraCalculatorSettings.ValidOrthographicSizes.Min, playerCruiserBowSlotXPosition);

            // AI cruiser naval factory
            float aiCruiserBowSlotXPosition = aiCruiser.Position.x - aiCruiser.Size.x / 2;
            AINavalFactoryTarget = CreateTarget(camera, cameraCalculator, cameraCalculatorSettings.ValidOrthographicSizes.Min, aiCruiserBowSlotXPosition);

            PlayerCruiserDeathTarget = CreateTarget(camera, cameraCalculator, CRUISER_DEATH_ORTHOGRAPHIC_SIZE, playerCruiser.Position.x);
            PlayerCruiserNukedTarget = CreateTarget(camera, cameraCalculator, NUKE_ORTHOGRAPHIC_SIZE, playerCruiser.Position.x);

            AICruiserDeathTarget = CreateTarget(camera, cameraCalculator, CRUISER_DEATH_ORTHOGRAPHIC_SIZE, aiCruiser.Position.x);
            AICruiserNukedTarget = CreateTarget(camera, cameraCalculator, NUKE_ORTHOGRAPHIC_SIZE, aiCruiser.Position.x);
        }

        private ICameraTarget FindCruiserTarget(ICamera camera, ICameraCalculator cameraCalculator, ICruiser cruiser)
        {
            float targetOrthographicSize = cameraCalculator.FindCameraOrthographicSize(cruiser);
            Vector3 targetPosition = cameraCalculator.FindCruiserCameraPosition(cruiser, targetOrthographicSize, camera.Position.z);
            return new CameraTarget(targetPosition, targetOrthographicSize);
        }

        private ICameraTarget CreateTarget(ICamera camera, ICameraCalculator cameraCalculator, float orthographicSize, float xPosition)
        {
            float yPosition = cameraCalculator.FindCameraYPosition(orthographicSize);
            Vector3 position = new Vector3(xPosition, yPosition, camera.Position.z);
            return new CameraTarget(position, orthographicSize);
        }
    }
}