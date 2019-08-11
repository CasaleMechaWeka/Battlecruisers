using System;
using UnityEngine;

namespace BattleCruisers.Effects
{
    public interface IBroadcastingParticleSystem
    {
        event EventHandler Stopped;

        ParticleSystem ParticleSystem { get; }
    }
}