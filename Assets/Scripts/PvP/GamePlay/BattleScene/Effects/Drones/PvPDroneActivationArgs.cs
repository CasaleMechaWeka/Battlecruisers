using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones
{
    public struct PvPDroneActivationArgs
    {
        public Vector2 Position { get; }
        public PvPFaction Faction { get; }

        public PvPDroneActivationArgs(Vector2 position, PvPFaction faction)
        {
            Position = position;
            Faction = faction;
        }
    }
}