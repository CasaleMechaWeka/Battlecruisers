using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Effects.Drones
{
    public struct DroneActivationArgs
    {
        public Vector2 Position { get; }

        // FELIX  Remove
        public bool PlayAudio { get; }

        public Faction Faction { get; }

        public DroneActivationArgs(Vector2 position, bool playAudio, Faction faction)
        {
            Position = position;
            PlayAudio = playAudio;
            Faction = faction;
        }
    }
}