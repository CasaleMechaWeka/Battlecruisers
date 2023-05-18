using System.Collections;
using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;



namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public abstract class PvPBattleSceneHelper : IPvPBattleSceneHelper
    {
        protected readonly IApplicationModel _appModel;
        public abstract IPvPBuildingCategoryPermitter BuildingCategoryPermitter { get; }

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

