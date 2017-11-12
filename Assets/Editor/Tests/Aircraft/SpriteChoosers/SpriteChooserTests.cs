using System.Collections.Generic;
using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Utils.UIWrappers;
using NSubstitute;
using NUnit.Framework;
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
        private float _maxVelocityInMPerS;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _assigner = Substitute.For<IAssigner>();
            _assignerFactory = Substitute.For<IAssignerFactory>();

			_sprite = Substitute.For<ISpriteWrapper>();
            _sprites = new List<ISpriteWrapper>()
			{
				_sprite
			};
   
            _assignerFactory.CreateRecursiveProportionAssigner(_sprites.Count).Returns(_assigner);

            _maxVelocityInMPerS = 5;

            _chooser = new SpriteChooser(_assignerFactory, _sprites, _maxVelocityInMPerS);
            _assignerFactory.Received().CreateRecursiveProportionAssigner(_sprites.Count);
        }

        [Test]
        public void ChooseSprite_TooHighVelocity_Throws()
        {
            Vector2 velocity = new Vector2(5.1f, 0);
            Assert.Throws<UnityAsserts.AssertionException>(() => _chooser.ChooseSprite(velocity));
        }

        [Test]
        public void ChooseSprite_InvalidIndexAssigned_Throws()
        {
            Vector2 velocity = new Vector2(5, 0);

            float proportion = velocity.magnitude / _maxVelocityInMPerS;
            int invalidIndex = _sprites.Count;

            _assigner.Assign(proportion).Returns(invalidIndex);

            Assert.Throws<UnityAsserts.AssertionException>(() => _chooser.ChooseSprite(velocity));
        }

        [Test]
        public void ChooseSprite_ReturnsSprite()
        {
            Vector2 velocity = new Vector2(5, 0);

            float proportion = velocity.magnitude / _maxVelocityInMPerS;
            int validIndex = 0;

            _assigner.Assign(proportion).Returns(validIndex);

            ISpriteWrapper spriteReturned = _chooser.ChooseSprite(velocity);
            Assert.AreSame(_sprite, spriteReturned);
        }
    }
}
