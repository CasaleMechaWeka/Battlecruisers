using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Trails
{
    public class PvPMissileTrailController : MonoBehaviour, IPvPProjectileTrail
    {
        public SpriteRenderer glow, missileFlare;
        public TrailRenderer trail;
        public BroadcastingParticleSystem optionalParticleEffect;

        public void Initialise()
        {
            PvPHelper.AssertIsNotNull(glow, missileFlare, trail);
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
            if (optionalParticleEffect != null)
            {
                optionalParticleEffect.Stop();
            }

            glow.enabled = false;
            missileFlare.enabled = false;
            trail.enabled = false;
        }

        public void SetVisibleTrail(bool isVisible)
        {
            trail.enabled = isVisible;
            if (optionalParticleEffect != null)
            {
                if (isVisible)
                {
                    optionalParticleEffect.Play();
                }
                else
                {
                    optionalParticleEffect.Stop();
                }
            }
        }
    }
}