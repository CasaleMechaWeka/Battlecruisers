using System;

namespace BattleCruisers.Effects.ParticleSystems
{
    public class DummyBroadcastingParticleSystem : IBroadcastingParticleSystem
    {
        public event EventHandler Stopped;

        public void Play() { }
        public void Stop() { }
    }
}