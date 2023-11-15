using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters
{
    public interface IPvPBroadcastingFilter
    {
        event EventHandler PotentialMatchChange;

        bool IsMatch { get; }
    }

    public interface IPvPBroadcastingFilter<TPvPElement> : IPvPFilter<TPvPElement>
    {
        event EventHandler PotentialMatchChange;
    }
}
