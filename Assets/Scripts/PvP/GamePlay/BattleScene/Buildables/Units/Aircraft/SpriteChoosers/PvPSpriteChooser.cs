using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers
{
    public class PvPSpriteChooser : IPvPSpriteChooser
    {
        private readonly IPvPAssigner _assigner;
        private readonly IList<IPvPSpriteWrapper> _sprites;
        private readonly IPvPVelocityProvider _maxVelocityProvider;

        public PvPSpriteChooser(
            IPvPAssignerFactory assignerFactory,
            IList<IPvPSpriteWrapper> sprites,
            IPvPVelocityProvider maxVelocityProvider)
        {
            PvPHelper.AssertIsNotNull(assignerFactory, sprites, maxVelocityProvider);
            Assert.IsTrue(sprites.Count > 0);

            _sprites = sprites;
            _maxVelocityProvider = maxVelocityProvider;

            _assigner = assignerFactory.CreateAssigner(sprites.Count);
        }

        public (IPvPSpriteWrapper, int) ChooseSprite(Vector2 velocity)
        {
            float magnitude = velocity.magnitude;

            if (magnitude > _maxVelocityProvider.VelocityInMPerS)
            {
                magnitude = _maxVelocityProvider.VelocityInMPerS;
            }

            float proportion = magnitude / _maxVelocityProvider.VelocityInMPerS;
            int spriteIndex = _assigner.Assign(proportion);

            Assert.IsTrue(spriteIndex < _sprites.Count && spriteIndex >= 0);
            return (_sprites[spriteIndex], spriteIndex);
        }

        public IPvPSpriteWrapper ChooseSprite(int index)
        {
            Assert.IsTrue(index < _sprites.Count && index >= 0);
            return _sprites[index];
        }
    }
}
