using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Movement
{
    public class MovementEffectInitialiser : MonoBehaviour, IMovementEffectInitialiser
    {

        public IMovementEffects CreateMovementEffects()
        {
            Animator animator = GetComponent<Animator>();
            Assert.IsNotNull(animator);

            BroadcastingParticleSystem particleSystem = GetComponentInChildren<BroadcastingParticleSystem>();
            // particleSystem may be null
            if (particleSystem != null)
            {
                particleSystem.Initialise();
            }

            return
                new ShipMovementEffect(
                    new AnimatorBC(animator),
                    (IBroadcastingParticleSystem)particleSystem ?? new DummyBroadcastingParticleSystem());
        }
    }
}