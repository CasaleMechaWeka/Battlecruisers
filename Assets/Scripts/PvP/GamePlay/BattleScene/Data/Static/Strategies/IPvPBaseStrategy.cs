using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys.Wrappers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies
{
    public interface IPvPBaseStrategy
    {
        IList<IPvPPrefabKeyWrapper> BuildOrder { get; }
    }
}
