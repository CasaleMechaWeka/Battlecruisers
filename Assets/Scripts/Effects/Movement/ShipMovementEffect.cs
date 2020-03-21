using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Effects.Movement
{
    // FELIX  Test
    // FELIX  Handle initial show and ending deactivation too?
    public class ShipMovementEffect : IMovementEffects
    {
        private readonly IAnimator _animator;
        private readonly IBroadcastingParticleSystem _particleSystem;

        private const string MOVEMENT_ANIMATION_STATE = "MovementAnimation";

        public ShipMovementEffect(IAnimator animator, IBroadcastingParticleSystem particleSystem)
        {
            Helper.AssertIsNotNull(animator, particleSystem);

            _animator = animator;
            _particleSystem = particleSystem;

            // Reset animation to start (for when ship is recycled and animation is not in starting position)
            _animator.Play(MOVEMENT_ANIMATION_STATE, layer: -1, normalizedTime: 0);
            StopEffects();
        }

        public void StartEffects()
        {
            _animator.Speed = 1;
            _particleSystem.Play();
        }

        public void StopEffects()
        {
            _animator.Speed = 0;
            _particleSystem.Stop();
        }
    }
}