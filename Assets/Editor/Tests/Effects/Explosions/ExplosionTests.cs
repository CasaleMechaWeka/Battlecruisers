using BattleCruisers.Effects.Explosions;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.Effects.Explosions
{
    public class ExplosionTests
    {
        private IPoolable<Vector3> _explosion;
        private IGameObject _controller;
        private IBroadcastingParticleSystem _particleSystem1, _particleSystem2;
        private IBroadcastingParticleSystem[] _particleSystems;
        private SynchronizedParticleSystemsController _system1, _system2;
        private SynchronizedParticleSystemsController[] _synchronizedSystems;
        private int _deactivatedCount;

        [SetUp]
        public void TestSetup()
        {
            _controller = Substitute.For<IGameObject>();
            _particleSystem1 = Substitute.For<IBroadcastingParticleSystem>();
            _particleSystem2 = Substitute.For<IBroadcastingParticleSystem>();
            _particleSystems = new IBroadcastingParticleSystem[]
            {
                _particleSystem1,
                _particleSystem2
            };
            _system1 = Substitute.For<SynchronizedParticleSystemsController>();
            _system2 = Substitute.For<SynchronizedParticleSystemsController>();
            _synchronizedSystems = new SynchronizedParticleSystemsController[]
            {
                _system1,
                _system2
            };

            _explosion = new Explosion(_controller, _particleSystems, _synchronizedSystems);

            _deactivatedCount = 0;
            _explosion.Deactivated += (sender, e) => _deactivatedCount++;
        }

        [Test]
        public void InitialState()
        {
            _controller.Received().IsVisible = false;
        }

        [Test]
        public void Activate()
        {
            Vector3 position = new Vector3(17, 15, 13);

            _explosion.Activate(position);

            _controller.Received().Position = position;
            _controller.Received().IsVisible = true;
            _system1.Received().ResetSeed();
            _system2.Received().ResetSeed();
            _particleSystem1.Received().Play();
            _particleSystem2.Received().Play();
        }

        [Test]
        public void ParticleSystem_Stopped_NotLast()
        {
            _controller.ClearReceivedCalls();

            _particleSystem1.Stopped += Raise.Event();

            Assert.AreEqual(0, _deactivatedCount);
            _controller.DidNotReceive().IsVisible = false;
        }

        [Test]
        public void ParticleSystem_Stopped_Last()
        {
            _controller.ClearReceivedCalls();

            _particleSystem1.Stopped += Raise.Event();
            _particleSystem2.Stopped += Raise.Event();

            Assert.AreEqual(1, _deactivatedCount);
            _controller.Received().IsVisible = false;
        }

        [Test]
        public void ParticleSystem_Stopped_PastLast()
        {
            ParticleSystem_Stopped_Last();

            _controller.ClearReceivedCalls();
            _deactivatedCount = 0;

            // Stopped count should have been reset, so this should not trigger a deactivated event
            _particleSystem1.Stopped += Raise.Event();

            Assert.AreEqual(0, _deactivatedCount);
            _controller.DidNotReceive().IsVisible = false;
        }
    }
}