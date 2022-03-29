using BattleCruisers.Effects;
using BattleCruisers.Effects.Deaths;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Tests.Effects.Deaths
{
    public class ShipDeathTests
    {
        private IShipDeath _shipDeath;
        private IGameObject _shipDeathController;
        private IBroadcastingAnimation _sinkingAnimation;
        private IList<IParticleSystemGroup> _effects;
        private IParticleSystemGroup _effect;

        /*[SetUp]
        public void TestSetup()
        {
            _shipDeathController = Substitute.For<IGameObject>();
            _sinkingAnimation = Substitute.For<IBroadcastingAnimation>();
            _effect = Substitute.For<IParticleSystemGroup>();
            _effects = new List<IParticleSystemGroup>()
            {
                _effect
            };

            _shipDeath = new ShipDeath(_shipDeathController, _sinkingAnimation, _effects);
        }

        [Test]
        public void InitialState()
        {
            _shipDeathController.Received().IsVisible = false;
        }

        [Test]
        public void Activate()
        {
            _shipDeathController.ClearReceivedCalls();
            Vector3 position = new Vector3(1, 2, 3);

            _shipDeath.Activate(position);

            _shipDeathController.Received().IsVisible = true;
            _shipDeathController.Received().Position = position;
            _sinkingAnimation.Received().Play();
            _effect.Received().Play();
        }

        [Test]
        public void AnimationDone()
        {
            int deactivatedCount = 0;
            _shipDeath.Deactivated += (sender, e) => deactivatedCount++;
            _shipDeathController.ClearReceivedCalls();

            _sinkingAnimation.AnimationDone += Raise.Event();

            _shipDeathController.Received().IsVisible = false;
            Assert.AreEqual(1, deactivatedCount);
        }
        */
    }
}