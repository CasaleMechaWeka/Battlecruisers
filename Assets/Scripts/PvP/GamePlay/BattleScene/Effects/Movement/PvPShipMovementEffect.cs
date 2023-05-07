using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.ParticleSystems;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Movement
{
    public class PvPShipMovementEffect : IPvPMovementEffect
    {
        private readonly IPvPGameObject _gameObject;
        private readonly IPvPAnimator _animator;
        private readonly IPvPBroadcastingParticleSystem _particleSystem;

        public const string MOVEMENT_ANIMATION_STATE = "MovementAnimation";

        public PvPShipMovementEffect(
            IPvPGameObject gameObject,
            IPvPAnimator animator,
            IPvPBroadcastingParticleSystem particleSystem)
        {
            PvPHelper.AssertIsNotNull(gameObject, animator, particleSystem);

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