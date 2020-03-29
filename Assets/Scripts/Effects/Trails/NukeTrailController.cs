using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Effects.Trails
{
    public class NukeTrailController : MonoBehaviour, IProjectileTrail
    {
        private IList<BroadcastingParticleSystem> _effects;
        public BroadcastingParticleSystem pulsingGlow, fireExhaust, smoke;
        public SpriteRenderer constantGlow;

        public void Initialise()
        {
            Helper.AssertIsNotNull(pulsingGlow, fireExhaust, smoke, constantGlow);

            _effects = new List<BroadcastingParticleSystem>()
            {
                pulsingGlow,
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

            constantGlow.enabled = true;
        }

        public void HideEffects()
        {
            foreach (IBroadcastingParticleSystem effect in _effects)
            {
                effect.Stop();
            }

            constantGlow.enabled = false;
        }
    }
}