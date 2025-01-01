using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones
{
    public struct PvPDroneActivationArgs
    {
        public Vector2 Position { get; }
        public Faction Faction { get; }

        public PvPDroneActivationArgs(Vector2 position, Faction faction)
        {
            Position = position;
            Faction = faction;
        }
    }
}