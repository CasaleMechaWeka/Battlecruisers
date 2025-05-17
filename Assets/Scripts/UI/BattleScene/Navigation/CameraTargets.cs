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
        public CameraTarget PlayerCruiserTarget { get; }
        public CameraTarget PlayerCruiserDeathTarget { get; }
        public CameraTarget PlayerCruiserNukedTarget { get; }
        public CameraTarget PlayerNavalFactoryTarget { get; }
        public CameraTarget EnemyCruiserTarget { get; }
        public CameraTarget EnemyCruiserDeathTarget { get; }
        public CameraTarget EnemyCruiserNukedTarget { get; }
        public CameraTarget EnemyNavalFactoryTarget { get; }
        public CameraTarget MidLeftTarget { get; }
        public CameraTarget OverviewTarget { get; }

        private const float CRUISER_DEATH_ORTHOGRAPHIC_SIZE = 10;
        private const float MID_ORTHOGRAPHIC_SIZE = 15;
        private const float NUKE_ORTHOGRAPHIC_SIZE = 30;

        public CameraTargets(
            CameraCalculator cameraCalculator,
            CameraCalculatorSettings cameraCalculatorSettings,
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            ICamera camera)
        {
            Helper.AssertIsNotNull(cameraCalculator, cameraCalculatorSettings, playerCruiser, aiCruiser, camera);

            PlayerCruiserTarget = FindCruiserTarget(camera, cameraCalculator, playerCruiser);
            EnemyCruiserTarget = FindCruiserTarget(camera, cameraCalculator, aiCruiser);

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
            EnemyNavalFactoryTarget = CreateTarget(camera, cameraCalculator, cameraCalculatorSettings.ValidOrthographicSizes.Min, aiCruiserBowSlotXPosition);

            PlayerCruiserDeathTarget = CreateTarget(camera, cameraCalculator, CRUISER_DEATH_ORTHOGRAPHIC_SIZE, playerCruiser.Position.x);
            PlayerCruiserNukedTarget = CreateTarget(camera, cameraCalculator, NUKE_ORTHOGRAPHIC_SIZE, playerCruiser.Position.x);

            EnemyCruiserDeathTarget = CreateTarget(camera, cameraCalculator, CRUISER_DEATH_ORTHOGRAPHIC_SIZE, aiCruiser.Position.x);
            EnemyCruiserNukedTarget = CreateTarget(camera, cameraCalculator, NUKE_ORTHOGRAPHIC_SIZE, aiCruiser.Position.x);
        }

        private CameraTarget FindCruiserTarget(ICamera camera, CameraCalculator cameraCalculator, ICruiser cruiser)
        {
            float targetOrthographicSize = cameraCalculator.FindCameraOrthographicSize(cruiser) + 0.75f;
            Vector3 targetPosition = cameraCalculator.FindCruiserCameraPosition(cruiser, targetOrthographicSize, camera.Position.z);
            return new CameraTarget(targetPosition, targetOrthographicSize);
        }

        private CameraTarget CreateTarget(ICamera camera, CameraCalculator cameraCalculator, float orthographicSize, float xPosition)
        {
            float yPosition = cameraCalculator.FindCameraYPosition(orthographicSize);
            Vector3 position = new Vector3(xPosition, yPosition, camera.Position.z);
            return new CameraTarget(position, orthographicSize);
        }
    }
}