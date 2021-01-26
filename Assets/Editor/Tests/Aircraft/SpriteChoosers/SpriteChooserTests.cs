using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Aircraft.SpriteChoosers
{
    public class SpriteChooserTests
    {
        private ISpriteChooser _chooser;
        private IAssignerFactory _assignerFactory;
        private IAssigner _assigner;
        private IList<ISpriteWrapper> _sprites;
        private ISpriteWrapper _sprite;
        private IVelocityProvider _maxVelocityProvider;

        [SetUp]
        public void SetuUp()
        {
            _assigner = Substitute.For<IAssigner>();
            _assignerFactory = Substitute.For<IAssignerFactory>();

			_sprite = Substitute.For<ISpriteWrapper>();
            _sprites = new List<ISpriteWrapper>()
			{
				_sprite
			};
   
            _assignerFactory.CreateAssigner(_sprites.Count).Returns(_assigner);

            _maxVelocityProvider = Substitute.For<IVelocityProvider>();
            _maxVelocityProvider.VelocityInMPerS.Returns(5);

            _chooser = new SpriteChooser(_assignerFactory, _sprites, _maxVelocityProvider);
            _assignerFactory.Received().CreateAssigner(_sprites.Count);
        }

        [Test]
        public void ChooseSprite_InvalidIndexAssigned_Throws()
        {
            Vector2 velocity = new Vector2(5, 0);

            float proportion = velocity.magnitude / _maxVelocityProvider.VelocityInMPerS;
            int invalidIndex = _sprites.Count;

            _assigner.Assign(proportion).Returns(invalidIndex);

            Assert.Throws<UnityAsserts.AssertionException>(() => _chooser.ChooseSprite(velocity));
        }

        [Test]
        public void ChooseSprite_ReturnsSprite()
        {
            Vector2 velocity = new Vector2(5, 0);

            float proportion = velocity.magnitude / _maxVelocityProvider.VelocityInMPerS;
            int validIndex = 0;

            _assigner.Assign(proportion).Returns(validIndex);

            ISpriteWrapper spriteReturned = _chooser.ChooseSprite(velocity);
            Assert.AreSame(_sprite, spriteReturned);
        }

        [Test]
        public void ChooseSprite_TooHighVelocity_CapsVelocity()
        {
            Vector2 velocity = new Vector2(5.1f, 0);

            float proportion = 1;
            int validIndex = 0;

            _assigner.Assign(proportion).Returns(validIndex);

            ISpriteWrapper spriteReturned = _chooser.ChooseSprite(velocity);
            Assert.AreSame(_sprite, spriteReturned);
        }
    }
}
