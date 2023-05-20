using System.Collections;
using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public abstract class PvPBattleSceneHelper : IPvPBattleSceneHelper
    {
        protected readonly IApplicationModel _appModel;
        protected readonly IPvPBuildProgressCalculatorFactory _calculatorFactory;
        private readonly IPvPPrefabFetcher _prefabFetcher;
        private readonly ILocTable _storyStrings;
        public virtual IPvPPrefabKey PlayerACruiser => /* _appModel.DataProvider.GameModel.PlayerLoadout.Hull */ new PvPHullKey("PvPYeti");
        public virtual IPvPPrefabKey PlayerBCruiser => /* _appModel.DataProvider.GameModel.PlayerLoadout.Hull */ new PvPHullKey("PvPRaptor");
        public abstract IPvPBuildingCategoryPermitter BuildingCategoryPermitter { get; }
        public abstract IPvPBuildProgressCalculator CreateAICruiserBuildProgressCalculator();
        public abstract IPvPSlotFilter CreateHighlightableSlotFilter();
        public abstract IPvPBuildProgressCalculator CreatePlayerCruiserBuildProgressCalculator();
        public abstract IPvPUserChosenTargetHelper CreateUserChosenTargetHelper(IPvPUserChosenTargetManager playerCruiserUserChosenTargetManager /*, IPvPPrioritisedSoundPlayer soundPlayer, IPvPTargetIndicator targetIndicator*/);


        protected PvPBattleSceneHelper(
            IApplicationModel appModel,
            IPvPPrefabFetcher prefabFetcher,
            ILocTable storyString
            )
        {
            _appModel = appModel;
            _prefabFetcher = prefabFetcher;
            _storyStrings = storyString;
            _calculatorFactory
                 = new PvPBuildProgressCalculatorFactory(
                   new PvPBuildSpeedCalculator());
        }
        public virtual IPvPLevel GetPvPLevel()
        {
            return _appModel.DataProvider.GetPvPLevel(Map.PracticeWreckyards);
            // return _appModel.DataProvider.GetPvPLevel();
        }


    }
}

