using System;

namespace BattleCruisers.Effects
{
    public interface IBroadcastingParticleSystem
    {
        event EventHandler Stopped;
    }
}