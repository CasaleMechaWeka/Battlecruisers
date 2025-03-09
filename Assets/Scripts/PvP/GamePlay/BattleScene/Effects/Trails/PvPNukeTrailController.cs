using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Trails
{
    public class PvPNukeTrailController : MonoBehaviour, IPvPProjectileTrail
    {
        private IList<PvPBroadcastingParticleSystem> _effects;
        public PvPBroadcastingParticleSystem fireExhaust, smoke, flame;
        public SpriteRenderer constantGlow, fireJet;

        public void Initialise()
        {
            PvPHelper.AssertIsNotNull(fireExhaust, smoke, flame);

            _effects = new List<PvPBroadcastingParticleSystem>()
            {
                fireExhaust,
                flame,
                smoke
            };
            foreach (PvPBroadcastingParticleSystem effect in _effects)
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

        public void SetVisibleTrail(bool isVisible)
        {
            smoke.enabled = isVisible;
        }
    }
}