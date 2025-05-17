using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public class PvPCameraTargets : ICameraTargets
    {
        public ICameraTarget PlayerCruiserTarget
        {
            get
            {
                return FindCruiserTarget(_camera, _cameraCalculator, _playerCruiser);
            }
        }
        public ICameraTarget PlayerCruiserDeathTarget { get; }
        public ICameraTarget PlayerCruiserNukedTarget { get; }
        public ICameraTarget PlayerNavalFactoryTarget { get; }

        public ICameraTarget EnemyCruiserTarget
        {
            get
            {
                return FindCruiserTarget(_camera, _cameraCalculator, _enemyCruiser);
            }
        }
        public ICameraTarget EnemyCruiserDeathTarget { get; }
        public ICameraTarget EnemyCruiserNukedTarget { get; }
        public ICameraTarget EnemyNavalFactoryTarget { get; }

        public ICameraTarget MidLeftTarget { get; }
        public ICameraTarget OverviewTarget { get; }

        private const float CRUISER_DEATH_ORTHOGRAPHIC_SIZE = 10;
        private const float MID_ORTHOGRAPHIC_SIZE = 15;
        private const float NUKE_ORTHOGRAPHIC_SIZE = 30;

        private IPvPCruiser _playerCruiser;
        private IPvPCruiser _enemyCruiser;
        private IPvPCameraCalculator _cameraCalculator;
        private ICamera _camera;

        public PvPCameraTargets(
            IPvPCameraCalculator cameraCalculator,
            CameraCalculatorSettings cameraCalculatorSettings,
            IPvPCruiser playerCruiser,
            IPvPCruiser enemyCruiser,
            ICamera camera)
        {
            PvPHelper.AssertIsNotNull(cameraCalculator, cameraCalculatorSettings, playerCruiser, enemyCruiser, camera);
            _playerCruiser = playerCruiser;
            _enemyCruiser = enemyCruiser;
            _camera = camera;
            _cameraCalculator = cameraCalculator;

            // PlayerCruiserTarget = FindCruiserTarget(camera, cameraCalculator, playerCruiser);
            // EnemyCruiserTarget = FindCruiserTarget(camera, cameraCalculator, enemyCruiser);

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
            float aiCruiserBowSlotXPosition = enemyCruiser.Position.x - enemyCruiser.Size.x / 2;
            EnemyNavalFactoryTarget = CreateTarget(camera, cameraCalculator, cameraCalculatorSettings.ValidOrthographicSizes.Min, aiCruiserBowSlotXPosition);

            PlayerCruiserDeathTarget = CreateTarget(camera, cameraCalculator, CRUISER_DEATH_ORTHOGRAPHIC_SIZE, playerCruiser.Position.x);
            PlayerCruiserNukedTarget = CreateTarget(camera, cameraCalculator, NUKE_ORTHOGRAPHIC_SIZE, playerCruiser.Position.x);

            EnemyCruiserDeathTarget = CreateTarget(camera, cameraCalculator, CRUISER_DEATH_ORTHOGRAPHIC_SIZE, enemyCruiser.Position.x);
            EnemyCruiserNukedTarget = CreateTarget(camera, cameraCalculator, NUKE_ORTHOGRAPHIC_SIZE, enemyCruiser.Position.x);
        }

        private ICameraTarget FindCruiserTarget(ICamera camera, IPvPCameraCalculator cameraCalculator, IPvPCruiser cruiser)
        {
            float targetOrthographicSize = cameraCalculator.FindCameraOrthographicSize(cruiser) + 0.75f;
            Vector3 targetPosition = cameraCalculator.FindCruiserCameraPosition(cruiser, targetOrthographicSize, camera.Position.z);
            return new CameraTarget(targetPosition, targetOrthographicSize);
        }

        private ICameraTarget CreateTarget(ICamera camera, IPvPCameraCalculator cameraCalculator, float orthographicSize, float xPosition)
        {
            float yPosition = cameraCalculator.FindCameraYPosition(orthographicSize);
            Vector3 position = new Vector3(xPosition, yPosition, camera.Position.z);
            return new CameraTarget(position, orthographicSize);
        }
    }
}