using System.Collections.Generic;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public class SpriteChooser : ISpriteChooser
    {
        private readonly IAssigner _assigner;
        private readonly IList<ISpriteWrapper> _sprites;
        private readonly IVelocityProvider _maxVelocityProvider;

        public SpriteChooser(
            IAssignerFactory assignerFactory,
            IList<ISpriteWrapper> sprites,
            IVelocityProvider maxVelocityProvider,
            float assignerBaseCutoff)
        {
            Helper.AssertIsNotNull(assignerFactory, sprites, maxVelocityProvider);
            Assert.IsTrue(sprites.Count > 0);

            _sprites = sprites;
            _maxVelocityProvider = maxVelocityProvider;

            _assigner = assignerFactory.CreateRecursiveProportionAssigner(sprites.Count, assignerBaseCutoff);
        }

        public ISpriteWrapper ChooseSprite(Vector2 velocity)
        {
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
