using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Trails
{
    public class PvPNukeTrailController : MonoBehaviour, IPvPProjectileTrail
    {
        private IList<PvPBroadcastingParticleSystem> _effects;
        public PvPBroadcastingParticleSystem pulsingGlow, fireExhaust, smoke;
        public SpriteRenderer constantGlow;

        public void Initialise()
        {
            PvPHelper.AssertIsNotNull(pulsingGlow, fireExhaust, smoke, constantGlow);

            _effects = new List<PvPBroadcastingParticleSystem>()
            {
                pulsingGlow,
                fireExhaust,
                smoke
            };
            foreach (PvPBroadcastingParticleSystem effect in _effects)
            {
                effect.Initialise();
            }
        }

        public void ShowAllEffects()
        {
            foreach (IPvPBroadcastingParticleSystem effect in _effects)
            {
                effect.Play();
            }

            constantGlow.enabled = true;
        }

        public void HideEffects()
        {
            foreach (IPvPBroadcastingParticleSystem effect in _effects)
            {
                effect.Stop();
            }
            constantGlow.enabled = false;
        }

        public void SetVisibleTrail(bool isVisible)
        {

        }
    }
}