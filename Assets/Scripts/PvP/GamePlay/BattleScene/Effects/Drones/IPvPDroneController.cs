using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Pools;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones
{
    public interface IPvPDroneController : IPvPPoolable<PvPDroneActivationArgs>
    {
        PvPFaction Faction { get; }

        event EventHandler Activated;

        void Deactivate();
    }
}