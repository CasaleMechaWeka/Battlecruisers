using UnityEngine;

namespace BattleCruisers.Effects
{
    // FELIX  Create Drones namespace?
    public struct DroneActivationArgs
    {
        public Vector2 Position { get; }
        public bool PlayAudio { get; }

        public DroneActivationArgs(Vector2 position, bool playAudio)
        {
            Position = position;
            PlayAudio = playAudio;
        }
    }
}