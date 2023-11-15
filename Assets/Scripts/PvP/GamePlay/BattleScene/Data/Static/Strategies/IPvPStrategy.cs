using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies.Requests;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies
{
    public interface IPvPStrategy
    {
        IPvPBaseStrategy BaseStrategy { get; }
        IEnumerable<IPvPOffensiveRequest> Offensives { get; }
    }
}
