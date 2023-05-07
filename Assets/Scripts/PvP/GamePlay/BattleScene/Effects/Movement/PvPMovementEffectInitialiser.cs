using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Movement
{
    public class PvPMovementEffectInitialiser : MonoBehaviour, IPvPMovementEffectInitialiser
    {
        public IPvPMovementEffect CreateMovementEffects()
        {
            Animator animator = GetComponent<Animator>();
            Assert.IsNotNull(animator);

            PvPBroadcastingParticleSystem particleSystem = GetComponentInChildren<PvPBroadcastingParticleSystem>();
            // particleSystem may be null
            if (particleSystem != null)
            {
                particleSystem.Initialise();
            }

            IPvPMovementEffect shipMovementEffect
                = new PvPShipMovementEffect(
                    new PvPGameObjectBC(gameObject),
                    new PvPAnimatorBC(animator),
                    (IPvPBroadcastingParticleSystem)particleSystem ?? new PvPDummyBroadcastingParticleSystem());

            shipMovementEffect.ResetAndHide();
            return shipMovementEffect;
        }
    }
}