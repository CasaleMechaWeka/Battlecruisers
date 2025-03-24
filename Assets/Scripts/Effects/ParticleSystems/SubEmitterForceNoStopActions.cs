using System;
using UnityEngine;

namespace BattleCruisers.Effects.ParticleSystems
{
    /// <summary>
    /// Attach this component to a GameObject with a ParticleSystem.
    /// Sets the particle system's stop action to None when activated.
    /// </summary>
    public class SubEmitterForceNoStopActions : MonoBehaviour
    {
        private void OnEnable()
        {
            ParticleSystem particleSystem = GetComponent<ParticleSystem>();
            
            if (particleSystem != null)
            {
                ParticleSystem.MainModule mainModule = particleSystem.main;
                mainModule.stopAction = 0;
            }
        }
    }
}