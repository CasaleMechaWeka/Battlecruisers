using BattleCruisers.Buildables;
using BattleCruisers.Utils.BattleScene.Pools;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones
{
    public interface IPvPDroneController : IPoolable<PvPDroneActivationArgs>
    {
        Faction Faction { get; }

        event EventHandler Activated;

        void Deactivate();
    }
}