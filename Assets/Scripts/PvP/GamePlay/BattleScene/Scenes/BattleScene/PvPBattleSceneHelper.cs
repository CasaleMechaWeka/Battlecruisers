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

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public abstract class PvPBattleSceneHelper : IPvPBattleSceneHelper
    {
        protected readonly IApplicationModel _appModel;
        protected readonly IPvPBuildProgressCalculatorFactory _calculatorFactory;
        public virtual IPvPPrefabKey PlayerCruiser => /* _appModel.DataProvider.GameModel.PlayerLoadout.Hull */ new PvPHullKey("dummy prefab");
        public abstract IPvPBuildingCategoryPermitter BuildingCategoryPermitter { get; }
        public abstract IPvPBuildProgressCalculator CreateAICruiserBuildProgressCalculator();
        public abstract IPvPSlotFilter CreateHighlightableSlotFilter();
        public abstract IPvPBuildProgressCalculator CreatePlayerCruiserBuildProgressCalculator();


        protected PvPBattleSceneHelper(
            IApplicationModel appModel,
            IPvPPrefabFetcher prefabFetcher,
            ILocTable storyString
            )
        {
            _appModel = appModel;
        }
        public virtual IPvPLevel GetPvPLevel()
        {
            return _appModel.DataProvider.GetPvPLevel(Map.PracticeWreckyards);
            // return _appModel.DataProvider.GetPvPLevel();
        }


    }
}

