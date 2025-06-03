using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Models;
using UnityEngine.Assertions;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Utils.Timers;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public class PvPBattleHelper : PvPBattleSceneHelper
    {
        private PvPUIManager _uiManager;

        private readonly BuildingCategoryFilter _buildingCategoryFilter;
        public override BuildingCategoryFilter BuildingCategoryPermitter => _buildingCategoryFilter;
        private ILoadout _loadout;

        public PvPBattleHelper() : base()
        {
            // _appModel = appModel;

            // For the real game want to enable all building categories :)
            _buildingCategoryFilter = new BuildingCategoryFilter();
            _buildingCategoryFilter.AllowAllCategories();

            _loadout = new Loadout(DataProvider.GameModel.PlayerLoadout.Hull,
                                   DataProvider.GameModel.PlayerLoadout.GetAllBuildings(),
                                   DataProvider.GameModel.PlayerLoadout.GetAllUnits());

            foreach (BuildingKey building in StaticData.GetBuildingsUnlockedBeforeLevel(32))
            {
                _loadout.AddBuilding(building);
                Debug.Log(building.ToString() + " " + _loadout.GetAllBuildings().Contains(building));
            }


            foreach (UnitKey unit in StaticData.GetUnitsUnlockedBeforeLevel(32))
                _loadout.AddUnit(unit);

            _loadout.AddUnit(StaticPrefabKeys.Units.Broadsword);
        }

        public override PvPUIManager CreateUIManager()
        {
            Assert.IsNull(_uiManager, "Should only call CreateUIManager() once");
            _uiManager = new PvPUIManager();
            return _uiManager;
        }

        public override void InitialiseUIManager(
            PvPCruiser playerCruiser,
            PvPCruiser enemyCruiser,
            IPvPBuildMenu buildMenu,
            IPvPItemDetailsManager detailsManager,
            IPrioritisedSoundPlayer soundPlayer,
            SingleSoundPlayer uiSoundPlayer)
        {
            Assert.IsNotNull(_uiManager, "Should only call after CreateUIManager()");
            _uiManager.Initialise(playerCruiser,
                enemyCruiser,
                buildMenu,
                detailsManager,
                soundPlayer,
                uiSoundPlayer);
        }

        public override IFilter<IPvPSlot> CreateHighlightableSlotFilter()
        {
            return new PvPFreeSlotFilter();
        }


        public override ILoadout GetPlayerLoadout()
        {
            return _loadout;
        }

        public override IPvPBuildProgressCalculator CreatePlayerACruiserBuildProgressCalculator()
        {
            return _calculatorFactory.CreatePlayerACruiserCalculator();
        }

        public override IPvPBuildProgressCalculator CreatePlayerBCruiserBuildProgressCalculator()
        {
            return _calculatorFactory.CreatePlayerBCruiserCalculator();
        }

        protected virtual Difficulty FindDifficulty()
        {
            return Difficulty.Harder;
        }

        public override IPvPButtonVisibilityFilters CreateButtonVisibilityFilters(DroneManager droneManager)
        {
            return
                new PvPButtonVisibilityFilters(
                    new PvPAffordableBuildableFilter(droneManager),
                    _buildingCategoryFilter,
                    new PvPChooseTargetButtonVisibilityFilter(),
                    new PvPDeleteButtonVisibilityFilter(),
                    new BroadcastingFilter(isMatch: true),
                    new StaticBroadcastingFilter(isMatch: true));
        }

        public override IPvPButtonVisibilityFilters CreateButtonVisibilityFilters(PvPCruiser playerCruiser)
        {
            return
                new PvPButtonVisibilityFilters(
                    new PvPAffordableBuildableFilter(playerCruiser),
                    _buildingCategoryFilter,
                    new PvPChooseTargetButtonVisibilityFilter(),
                    new PvPDeleteButtonVisibilityFilter(),
                    new BroadcastingFilter(isMatch: true),
                    new StaticBroadcastingFilter(isMatch: true));
        }

        public override IPrioritisedSoundPlayer GetBuildableButtonSoundPlayer(IPvPCruiser playerCruiser)
        {
            return PvPFactoryProvider.Sound.IPrioritisedSoundPlayer;
        }

        public override IUserChosenTargetHelper CreateUserChosenTargetHelper(
            IUserChosenTargetManager playerCruiserUserChosenTargetManager
            )
        {
            PvPHelper.AssertIsNotNull(playerCruiserUserChosenTargetManager);
            return new PvPUserChosenTargetHelper(playerCruiserUserChosenTargetManager);
        }

        public override IUserChosenTargetHelper CreateUserChosenTargetHelper(
            IUserChosenTargetManager playerCruiserUserChosenTargetManager,
            IPrioritisedSoundPlayer soundPlayer,
            TargetIndicatorController targetIndicator)
        {
            PvPHelper.AssertIsNotNull(playerCruiserUserChosenTargetManager);

            return new PvPUserChosenTargetHelper(playerCruiserUserChosenTargetManager, soundPlayer, targetIndicator);
        }


        public override IManagedDisposable CreateDroneEventSoundPlayer(IPvPCruiser playerCruiser, IDeferrer deferrer)
        {
            return
                new DroneEventSoundPlayer(
                    new DroneManagerMonitor(playerCruiser.DroneManager, deferrer),
                    PvPFactoryProvider.Sound.IPrioritisedSoundPlayer,
                    new Debouncer(TimeBC.Instance.RealTimeSinceGameStartProvider, debounceTimeInS: 20));
        }
    }
}

