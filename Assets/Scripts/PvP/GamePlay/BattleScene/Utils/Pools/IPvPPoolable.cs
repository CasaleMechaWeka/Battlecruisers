using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildable;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils
{
    public interface IPvPPoolable<TArgs>
    {
        event EventHandler Deactivated;

        void Activate(TArgs activationArgs);

        void Activate(TArgs activationArgs, Faction faction);
    }
}
