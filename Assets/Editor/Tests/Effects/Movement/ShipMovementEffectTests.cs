using BattleCruisers.Effects.Movement;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Effects.Movement
{
    public class ShipMovementEffectTests
    {
        private IMovementEffect _effect;
        private IGameObject _gameObject;
        private IAnimator _animator;
        private IBroadcastingParticleSystem _particleSystem;

        [SetUp]
        public void TestSetup()
        {
            _gameObject = Substitute.For<IGameObject>();
            _animator = Substitute.For<IAnimator>();
            _particleSystem = Substitute.For<IBroadcastingParticleSystem>();

            _effect = new ShipMovementEffect(_gameObject, _animator, _particleSystem);
        }

        [Test]
        public void Show()
        {
            _effect.Show();
            _gameObject.Received().IsVisible = true;
        }

        [Test]
        public void StartEffects()
        {
            _effect.StartEffects();

            _animator.Received().Speed = 1;
            _particleSystem.Received().Play();
        }

        [Test]
        public void StopEffects()
        {
            _effect.StopEffects();

            _animator.Received().Speed = 0;
            _particleSystem.Received().Stop();
        }

        [Test]
        public void ResetAndHide()
        {
            _effect.ResetAndHide();

            _animator.Received().Play(ShipMovementEffect.MOVEMENT_ANIMATION_STATE, layer: -1, normalizedTime: 0);
            _animator.Received().Speed = 0;
            _particleSystem.Received().Stop();
            _gameObject.Received().IsVisible = false;
        }
    }
}