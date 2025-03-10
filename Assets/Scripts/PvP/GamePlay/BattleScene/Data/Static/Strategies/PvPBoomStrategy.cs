using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys.Wrappers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies
{
    public class PvPBoomStrategy : IPvPBaseStrategy
    {
        public IList<IPvPPrefabKeyWrapper> BuildOrder => PvPStaticBuildOrders.PvPAdaptive.Boom;
    }
}
