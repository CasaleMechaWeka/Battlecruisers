using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Effects.Drones
{
    public struct DroneActivationArgs
    {
        public Vector2 Position { get; }
        public Faction Faction { get; }

        public DroneActivationArgs(Vector2 position, Faction faction)
        {
            Position = position;
            Faction = faction;
        }
    }
}