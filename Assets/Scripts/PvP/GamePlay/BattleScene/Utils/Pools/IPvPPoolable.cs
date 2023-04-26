using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;


namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Pools
{
    public interface IPvPPoolable<TPvPArgs>
    {
        event EventHandler Deactivated;

        void Activate(TPvPArgs activationArgs);

        void Activate(TPvPArgs activationArgs, PvPFaction faction);
    }
}
