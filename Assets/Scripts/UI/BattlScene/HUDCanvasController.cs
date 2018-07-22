using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.Cruisers;
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene
{
    public class HUDCanvasController : MonoBehaviour, IHUDCanvasController
    {
        private BuildingDetailsController _buildingDetails;
        public IBuildableDetails<IBuilding> BuildingDetails { get { return _buildingDetails; } }

        private UnitDetailsController _unitDetails;
        public IBuildableDetails<IUnit> UnitDetails { get { return _unitDetails; } }

        private CruiserDetailsController _cruiserDetails;
        public ICruiserDetails CruiserDetails { get { return _cruiserDetails; } }

        private CruiserInfoController _playerCruiserInfo;
        public ICruiserInfo PlayerCruiserInfo { get { return _playerCruiserInfo; } }

        private CruiserInfoController _aiCruiserInfo;

        private NavigationButtonsWrapper _navigationButtonWrapper;
        public INavigationButtonsWrapper NavigationButtonsWrapper { get { return _navigationButtonWrapper; } }

        private GameSpeedWrapper _gameSpeedWrapper;
        public IGameSpeedWrapper GameSpeedWrapper { get { return _gameSpeedWrapper; } }

        public void StaticInitialise()
        {
            _buildingDetails = GetComponentInChildren<BuildingDetailsController>(includeInactive: true);
            Assert.IsNotNull(_buildingDetails);

            _unitDetails = GetComponentInChildren<UnitDetailsController>(includeInactive: true);
            Assert.IsNotNull(_unitDetails);

            _cruiserDetails = GetComponentInChildren<CruiserDetailsController>(includeInactive: true);
            Assert.IsNotNull(_cruiserDetails);

            _playerCruiserInfo = transform.FindNamedComponent<CruiserInfoController>("PlayerCruiserInfo");
            _aiCruiserInfo = transform.FindNamedComponent<CruiserInfoController>("AICruiserInfo");

            _navigationButtonWrapper = GetComponentInChildren<NavigationButtonsWrapper>();
            Assert.IsNotNull(_navigationButtonWrapper);

            _gameSpeedWrapper = GetComponentInChildren<GameSpeedWrapper>();
            Assert.IsNotNull(_gameSpeedWrapper);
        }

        public void Initialise(
            ISpriteProvider spriteProvider, 
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            ICameraController cameraController,
            IBroadcastingFilter shouldNavigationBeEnabledFilter)
        {
            Helper.AssertIsNotNull(spriteProvider, playerCruiser, aiCruiser, cameraController, shouldNavigationBeEnabledFilter);

            _buildingDetails.Initialise(spriteProvider, playerCruiser.DroneManager, playerCruiser.RepairManager);
            _unitDetails.Initialise(playerCruiser.DroneManager, playerCruiser.RepairManager);
            _cruiserDetails.Initialise(playerCruiser.DroneManager, playerCruiser.RepairManager);

            _playerCruiserInfo.Initialise(playerCruiser);
            _aiCruiserInfo.Initialise(aiCruiser);

            _navigationButtonWrapper.Initialise(cameraController, shouldNavigationBeEnabledFilter);
            _gameSpeedWrapper.Initialise();
        }
    }
}
