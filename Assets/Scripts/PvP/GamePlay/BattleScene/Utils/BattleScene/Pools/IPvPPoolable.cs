using System;
using BattleCruisers.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools
{
    public interface IPvPPoolable<TArgs>
    {
        event EventHandler Deactivated;

        void Activate(TArgs activationArgs);

        void Activate(TArgs activationArgs, Faction faction);
    }
}