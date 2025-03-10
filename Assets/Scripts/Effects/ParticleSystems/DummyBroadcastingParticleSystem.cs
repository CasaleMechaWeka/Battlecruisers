using System;

namespace BattleCruisers.Effects.ParticleSystems
{
    public class DummyBroadcastingParticleSystem : IBroadcastingParticleSystem
    {
#pragma warning disable 67  // Unused event
        public event EventHandler Stopped;
#pragma warning restore 67  // Unused event

        public void Play() { }
        public void Stop() { }
    }
}