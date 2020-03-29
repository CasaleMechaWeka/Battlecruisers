using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Effects.Trails
{
    public class NukeTrailController : MonoBehaviour, IProjectileTrail
    {
        public BroadcastingParticleSystem glow, fireExhaust, smoke;
        private IList<BroadcastingParticleSystem> _effects;

        public void Initialise()
        {
            Helper.AssertIsNotNull(glow, fireExhaust, smoke);
            
            _effects = new List<BroadcastingParticleSystem>()
            {
                glow,
                fireExhaust,
                smoke
            };
            foreach (BroadcastingParticleSystem effect in _effects)
            {
                effect.Initialise();
            }
        }

        public void ShowAllEffects()
        {
            foreach (IBroadcastingParticleSystem effect in _effects)
            {
                effect.Play();
            }
        }

        public void HideAliveEffects()
        {
            foreach (IBroadcastingParticleSystem effect in _effects)
            {
                effect.Stop();
            }
        }

        // FELIX  Remove?
        public void HideAllEffects()
        {
            // empty
        }
    }
}