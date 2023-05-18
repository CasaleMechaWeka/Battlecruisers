using System.Collections;
using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using UnityEngine;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public class PvPBattleHelper : PvPBattleSceneHelper
    {

        private readonly IPvPPrefabFactory _prefabFactory;
        private readonly IPvPDeferrer _deferrer;


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
            _prefabFactory = prefabFactory;
            _deferrer = deferrer;


            // For the real game want to enable all building categories :)
            _buildingCategoryFilter = new PvPBuildingCategoryFilter();
            _buildingCategoryFilter.AllowAllCategories();
        }
    }
}

