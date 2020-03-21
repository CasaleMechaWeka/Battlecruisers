using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Effects.Movement
{
    // FELIX  Test
    public class ShipMovementEffect : IMovementEffect
    {
        private readonly IGameObject _gameObject;
        private readonly IAnimator _animator;
        private readonly IBroadcastingParticleSystem _particleSystem;

        private const string MOVEMENT_ANIMATION_STATE = "MovementAnimation";

        public ShipMovementEffect(
            IGameObject gameObject,
            IAnimator animator, 
            IBroadcastingParticleSystem particleSystem)
        {
            Helper.AssertIsNotNull(gameObject, animator, particleSystem);

            _gameObject = gameObject;
            _animator = animator;
            _particleSystem = particleSystem;
        }

        public void Show()
        {
            _gameObject.IsVisible = true;
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

        public void ResetAndHide()
        {
            // Reset animation to start (for when ship is recycled and animation is not in starting position)
            _animator.Play(MOVEMENT_ANIMATION_STATE, layer: -1, normalizedTime: 0);
            StopEffects();
            _gameObject.IsVisible = false;
        }
    }
}