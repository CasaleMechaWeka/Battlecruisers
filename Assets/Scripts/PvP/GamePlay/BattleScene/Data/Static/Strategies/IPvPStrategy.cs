using BattleCruisers.Data.Static.Strategies.Requests;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies
{
    public interface IPvPStrategy
    {
        IPvPBaseStrategy BaseStrategy { get; }
        IEnumerable<IOffensiveRequest> Offensives { get; }
    }
}
