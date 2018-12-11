using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Cruisers;
using BattleCruisers.UI.BattleScene.GameSpeed;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene
{
    // NEWUI  Class is deprecated :P
    public class HUDCanvasController : Panel, IHUDCanvasController
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
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            ICameraController cameraController,
            IBroadcastingFilter shouldNavigationBeEnabledFilter,
            IUserChosenTargetHelper userChosenTargetHelper,
            IFilter<ITarget> chooseTargetButtonVisibilityFilter,
            IFilter<ITarget> deleteButtonVisibilityFilter)
        {
            Helper.AssertIsNotNull(
                playerCruiser, 
                aiCruiser, 
                cameraController, 
                shouldNavigationBeEnabledFilter, 
                userChosenTargetHelper, 
                chooseTargetButtonVisibilityFilter,
                deleteButtonVisibilityFilter);

            _buildingDetails.Initialise(playerCruiser.DroneFocuser, playerCruiser.RepairManager, userChosenTargetHelper, chooseTargetButtonVisibilityFilter, deleteButtonVisibilityFilter);
            _unitDetails.Initialise(playerCruiser.DroneFocuser, playerCruiser.RepairManager, userChosenTargetHelper, chooseTargetButtonVisibilityFilter, deleteButtonVisibilityFilter);
            _cruiserDetails.Initialise(playerCruiser.DroneFocuser, playerCruiser.RepairManager, userChosenTargetHelper, chooseTargetButtonVisibilityFilter);

            _playerCruiserInfo.Initialise(playerCruiser);
            _aiCruiserInfo.Initialise(aiCruiser);

            _navigationButtonWrapper.Initialise(cameraController, shouldNavigationBeEnabledFilter);
            _gameSpeedWrapper.Initialise();
        }
    }
}
