using System.Collections;
using System.Collections.Generic;
using BattleCruisers.Data;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;



namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public abstract class PvPBattleSceneHelper : IPvPBattleSceneHelper
    {
        protected readonly IApplicationModel _appModel;

        protected PvPBattleSceneHelper(IApplicationModel appModel, IPvPPrefabFetcher prefabFetcher, ILocTable storyString)
        {

        }
        public virtual IPvPLevel GetPvPLevel()
        {
            return _appModel.DataProvider.GetPvPLevel(_appModel.SelectedPvPLevel);
        }
    }
}

