using System;

namespace BattleCruisers.Effects.ParticleSystems
{
    public interface IBroadcastingParticleSystem
    {
        event EventHandler Stopped;

        void Play();
        void Stop();
    }
}