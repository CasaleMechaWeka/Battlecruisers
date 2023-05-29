using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation
{
    public class PvPCameraTargets : IPvPCameraTargets
    {
        public IPvPCameraTarget PlayerCruiserTarget
        {
            get
            {
                return FindCruiserTarget(_camera, _cameraCalculator, _playerCruiser);
            }
        }
        public IPvPCameraTarget PlayerCruiserDeathTarget { get; }
        public IPvPCameraTarget PlayerCruiserNukedTarget { get; }
        public IPvPCameraTarget PlayerNavalFactoryTarget { get; }

        public IPvPCameraTarget EnemyCruiserTarget
        {
            get
            {
                return FindCruiserTarget(_camera, _cameraCalculator, _enemyCruiser);
            }
        }
        public IPvPCameraTarget EnemyCruiserDeathTarget { get; }
        public IPvPCameraTarget EnemyCruiserNukedTarget { get; }
        public IPvPCameraTarget EnemyNavalFactoryTarget { get; }

        public IPvPCameraTarget MidLeftTarget { get; }
        public IPvPCameraTarget OverviewTarget { get; }

        private const float CRUISER_DEATH_ORTHOGRAPHIC_SIZE = 10;
        private const float MID_ORTHOGRAPHIC_SIZE = 15;
        private const float NUKE_ORTHOGRAPHIC_SIZE = 30;

        private IPvPCruiser _playerCruiser;
        private IPvPCruiser _enemyCruiser;
        private IPvPCameraCalculator _cameraCalculator;
        private IPvPCamera _camera;

        public PvPCameraTargets(
            IPvPCameraCalculator cameraCalculator,
            IPvPCameraCalculatorSettings cameraCalculatorSettings,
            IPvPCruiser playerCruiser,
            IPvPCruiser enemyCruiser,
            IPvPCamera camera)
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
            OverviewTarget = new PvPCameraTarget(overviewPosition, cameraCalculatorSettings.ValidOrthographicSizes.Max);

            IPvPRange<float> midXPositions = cameraCalculator.FindValidCameraXPositions(MID_ORTHOGRAPHIC_SIZE);
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

        private IPvPCameraTarget FindCruiserTarget(IPvPCamera camera, IPvPCameraCalculator cameraCalculator, IPvPCruiser cruiser)
        {
            float targetOrthographicSize = cameraCalculator.FindCameraOrthographicSize(cruiser);
            Vector3 targetPosition = cameraCalculator.FindCruiserCameraPosition(cruiser, targetOrthographicSize, camera.Position.z);
            return new PvPCameraTarget(targetPosition, targetOrthographicSize);
        }

        private IPvPCameraTarget CreateTarget(IPvPCamera camera, IPvPCameraCalculator cameraCalculator, float orthographicSize, float xPosition)
        {
            float yPosition = cameraCalculator.FindCameraYPosition(orthographicSize);
            Vector3 position = new Vector3(xPosition, yPosition, camera.Position.z);
            return new PvPCameraTarget(position, orthographicSize);
        }
    }
}