using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Trails
{
    public class PvPMissileTrailController : MonoBehaviour, IPvPProjectileTrail
    {
        public SpriteRenderer glow, missileFlare;
        public TrailRenderer trail;
        public PvPBroadcastingParticleSystem optionalParticleEffect;

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
            trail.enabled = true;
            trail.Clear();

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