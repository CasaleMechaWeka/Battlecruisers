using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public class SpriteChooser
    {
        private readonly LinearProportionAssigner _assigner;
        private readonly IList<Sprite> _sprites;
        private readonly IVelocityProvider _maxVelocityProvider;

        public SpriteChooser(
            IList<Sprite> sprites,
            IVelocityProvider maxVelocityProvider)
        {
            Helper.AssertIsNotNull(sprites);
            Assert.IsTrue(sprites.Count > 0);

            _sprites = sprites;

            if (sprites.Count > 1)
            {
                Helper.AssertIsNotNull(maxVelocityProvider);
                _maxVelocityProvider = maxVelocityProvider;
                _assigner = new LinearProportionAssigner(sprites.Count);
            }

        }

        public Sprite ChooseSprite(Vector2 velocity)
        {
            if (_sprites.Count == 1)
                return _sprites[0];

            float magnitude = velocity.magnitude;

            if (magnitude > _maxVelocityProvider.VelocityInMPerS)
            {
                magnitude = _maxVelocityProvider.VelocityInMPerS;
            }

            float proportion = magnitude / _maxVelocityProvider.VelocityInMPerS;
            int spriteIndex = _assigner.Assign(proportion);

            Assert.IsTrue(spriteIndex < _sprites.Count && spriteIndex >= 0);
            return _sprites[spriteIndex];
        }
    }
}
