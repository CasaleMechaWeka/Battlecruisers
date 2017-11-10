using System.Collections.Generic;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    // FELIX  Wrap sprite, to make this testable :)
    // FELIX  Test
    public class SpriteChooser : ISpriteChooser
    {
        private readonly IAssigner _assigner;
        private readonly IList<Sprite> _sprites;
        private readonly float _maxVelocityInMPerS;

        public SpriteChooser(IAssignerFactory assignerFactory, IList<Sprite> sprites, float maxVelocityInMPerS)
        {
            Helper.AssertIsNotNull(assignerFactory, sprites);
            Assert.IsTrue(sprites.Count > 0);
            Assert.IsTrue(maxVelocityInMPerS > 0);

            _sprites = sprites;
            _maxVelocityInMPerS = maxVelocityInMPerS;

            _assigner = assignerFactory.CreateRecursiveProportionAssigner(sprites.Count);
        }

        public Sprite ChooseSprite(Vector2 velocity)
        {
            float magnitude = velocity.magnitude;
            Assert.IsTrue(magnitude <= _maxVelocityInMPerS);

            float proportion = magnitude / _maxVelocityInMPerS;
            int spriteIndex = _assigner.Assign(proportion);

            Assert.IsTrue(spriteIndex < _sprites.Count);
            return _sprites[spriteIndex];
        }
    }
}
