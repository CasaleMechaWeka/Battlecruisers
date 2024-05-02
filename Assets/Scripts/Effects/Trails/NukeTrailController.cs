using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Effects.Trails
{
    public class NukeTrailController : MonoBehaviour, IProjectileTrail
    {
        private IList<BroadcastingParticleSystem> _effects;
        public BroadcastingParticleSystem fireExhaust, smoke;
        public SpriteRenderer constantGlow, fireJet;

        public void Initialise()
        {
            Helper.AssertIsNotNull( fireExhaust, smoke, constantGlow);

            _effects = new List<BroadcastingParticleSystem>()
            {
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
            fireJet.enabled = true;
        }

        public void HideEffects()
        {
            foreach (IBroadcastingParticleSystem effect in _effects)
            {
                effect.Stop();
            }

            constantGlow.enabled = false;
            fireJet.enabled = false;
        }
    }
}