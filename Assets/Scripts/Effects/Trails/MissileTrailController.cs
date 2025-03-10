using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Effects.Trails
{
    public class MissileTrailController : MonoBehaviour, IProjectileTrail
    {
        public SpriteRenderer glow, missileFlare;
        public TrailRenderer trail;
        public BroadcastingParticleSystem optionalParticleEffect;

        public void Initialise()
        {
            Helper.AssertIsNotNull(glow, missileFlare, trail);
            if (optionalParticleEffect != null)
            {
                optionalParticleEffect.Initialise();
            }
        }

        public void ShowAllEffects()
        {
            glow.enabled = true;
            missileFlare.enabled = true;
            trail.Clear();
            trail.enabled = true;

            if (optionalParticleEffect != null)
            {
                optionalParticleEffect.Play();
            }
        }

        public void HideEffects()
        {
            glow.enabled = false;
            missileFlare.enabled = false;
            trail.enabled = false;

            if (optionalParticleEffect != null)
            {
                optionalParticleEffect.Stop();
            }
        }
    }
}