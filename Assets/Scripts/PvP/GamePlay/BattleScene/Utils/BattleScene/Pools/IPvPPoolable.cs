using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools
{
    public interface IPvPPoolable<TArgs>
    {
        event EventHandler Deactivated;

        void Activate(TArgs activationArgs);

        void Activate(TArgs activationArgs, PvPFaction faction);
    }
}