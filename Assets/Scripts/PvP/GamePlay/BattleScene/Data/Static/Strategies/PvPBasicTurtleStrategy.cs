using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys.Wrappers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies
{
    public class PvPBasicTurtleStrategy : IPvPBaseStrategy
    {
        public IList<IPvPPrefabKeyWrapper> BuildOrder => PvPStaticBuildOrders.Basic.Turtle;
    }
}
