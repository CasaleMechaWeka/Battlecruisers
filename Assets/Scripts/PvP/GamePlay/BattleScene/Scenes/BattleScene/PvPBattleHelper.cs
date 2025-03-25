using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
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

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public class PvPBattleHelper : PvPBattleSceneHelper
    {

        private readonly IPvPPrefabFactory _prefabFactory;
        private readonly IDeferrer _deferrer;

        private PvPUIManager _uiManager;

        protected DataProvider DataProvider => _appModel.DataProvider;


        private readonly BuildingCategoryFilter _buildingCategoryFilter;
        public override IBuildingCategoryPermitter BuildingCategoryPermitter => _buildingCategoryFilter;

        public PvPBattleHelper(
            IApplicationModel appModel,
            IPvPPrefabFactory prefabFactory,
            IDeferrer deferrer
        ) : base(appModel)
        {
            // _appModel = appModel;
            _prefabFactory = prefabFactory;
            _deferrer = deferrer;


            // For the real game want to enable all building categories :)
            _buildingCategoryFilter = new BuildingCategoryFilter();
            _buildingCategoryFilter.AllowAllCategories();
        }

        public override IPvPUIManager CreateUIManager()
        {
            Assert.IsNull(_uiManager, "Should only call CreateUIManager() once");
            _uiManager = new PvPUIManager();
            return _uiManager;
        }

        public override void InitialiseUIManager(PvPManagerArgs args)
        {
            Assert.IsNotNull(_uiManager, "Should only call after CreateUIManager()");
            _uiManager.Initialise(args);
        }

        public override IFilter<IPvPSlot> CreateHighlightableSlotFilter()
        {
            return new PvPFreeSlotFilter();
        }


        public override ILoadout GetPlayerLoadout()
        {
            return DataProvider.GameModel.PlayerLoadout;
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

        public override IPvPButtonVisibilityFilters CreateButtonVisibilityFilters(IDroneManager droneManager)
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
            return playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer;
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
            ITargetIndicator targetIndicator)
        {
            PvPHelper.AssertIsNotNull(playerCruiserUserChosenTargetManager);

            return new PvPUserChosenTargetHelper(playerCruiserUserChosenTargetManager, soundPlayer, targetIndicator);
        }


        public override IManagedDisposable CreateDroneEventSoundPlayer(IPvPCruiser playerCruiser, IDeferrer deferrer)
        {
            return
                new DroneEventSoundPlayer(
                    new DroneManagerMonitor(playerCruiser.DroneManager, deferrer),
                    playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer,
                    new Debouncer(TimeBC.Instance.RealTimeSinceGameStartProvider, debounceTimeInS: 20));
        }
    }
}

