using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Models;
using UnityEngine.Assertions;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public class PvPBattleHelper : PvPBattleSceneHelper
    {

        private readonly IPvPPrefabFactory _prefabFactory;
        private readonly IPvPDeferrer _deferrer;

        private PvPUIManager _uiManager;

        protected IDataProvider DataProvider => _appModel.DataProvider;


        private readonly PvPBuildingCategoryFilter _buildingCategoryFilter;
        public override IPvPBuildingCategoryPermitter BuildingCategoryPermitter => _buildingCategoryFilter;

        public PvPBattleHelper(
            IApplicationModel appModel,
            IPvPPrefabFetcher prefabFetcher,
            ILocTable storyStrings,
            IPvPPrefabFactory prefabFactory,
            IPvPDeferrer deferrer
        ) : base(appModel, prefabFetcher, storyStrings)
        {
            // _appModel = appModel;
            _prefabFactory = prefabFactory;
            _deferrer = deferrer;


            // For the real game want to enable all building categories :)
            _buildingCategoryFilter = new PvPBuildingCategoryFilter();
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

        public override IPvPSlotFilter CreateHighlightableSlotFilter()
        {
            return new PvPFreeSlotFilter();
        }


        public override ILoadout GetPlayerLoadout()
        {
            return DataProvider.GameModel.PlayerLoadout;
        }

        public override IPvPBuildProgressCalculator CreateAICruiserBuildProgressCalculator()
        {
            return _calculatorFactory.CreateIncrementalAICruiserCalculator(FindDifficulty(), _appModel.SelectedLevel);
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


        public override IPvPButtonVisibilityFilters CreateButtonVisibilityFilters(IPvPDroneManager droneManager)
        {
            return
                new PvPButtonVisibilityFilters(
                    new PvPAffordableBuildableFilter(droneManager),
                    _buildingCategoryFilter,
                    new PvPChooseTargetButtonVisibilityFilter(),
                    new PvPDeleteButtonVisibilityFilter(),
                    new PvPBroadcastingFilter(isMatch: true),
                    new PvPStaticBroadcastingFilter(isMatch: true));
        }

        public override IPvPButtonVisibilityFilters CreateButtonVisibilityFilters()
        {
            return
                new PvPButtonVisibilityFilters(
                    new PvPAffordableBuildableFilter(),
                    _buildingCategoryFilter,
                    new PvPChooseTargetButtonVisibilityFilter(),
                    new PvPDeleteButtonVisibilityFilter(),
                    new PvPBroadcastingFilter(isMatch: true),
                    new PvPStaticBroadcastingFilter(isMatch: true));
        }

        public override IPvPPrioritisedSoundPlayer GetBuildableButtonSoundPlayer(IPvPCruiser playerCruiser)
        {
            return playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer;
        }

        public override IPvPUserChosenTargetHelper CreateUserChosenTargetHelper(
    IPvPUserChosenTargetManager playerCruiserUserChosenTargetManager
    // IPrioritisedSoundPlayer soundPlayer,
    // ITargetIndicator targetIndicator
    )
        {
            PvPHelper.AssertIsNotNull(playerCruiserUserChosenTargetManager);

            return new PvPUserChosenTargetHelper(playerCruiserUserChosenTargetManager /*, soundPlayer, targetIndicator*/);
        }



        public override IPvPUserChosenTargetHelper CreateUserChosenTargetHelper(
            IPvPUserChosenTargetManager playerCruiserUserChosenTargetManager,
            IPvPPrioritisedSoundPlayer soundPlayer,
            IPvPTargetIndicator targetIndicator)
        {
            PvPHelper.AssertIsNotNull(playerCruiserUserChosenTargetManager);

            return new PvPUserChosenTargetHelper(playerCruiserUserChosenTargetManager, soundPlayer, targetIndicator);
        }




    }
}

