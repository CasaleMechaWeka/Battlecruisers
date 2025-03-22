using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public class SpriteChooserFactory
    {
        private readonly SpriteProvider _spriteProvider;

        public SpriteChooserFactory(SpriteProvider spriteProvider)
        {
            Helper.AssertIsNotNull(spriteProvider);

            _spriteProvider = spriteProvider;
        }

        public async Task<ISpriteChooser> CreateAircraftSpriteChooserAsync(PrefabKeyName prefabKeyName, IVelocityProvider maxVelocityProvider)
        {
            IList<Sprite> aircraftSprites = await _spriteProvider.GetAircraftSpritesAsync(prefabKeyName);
            return new SpriteChooser(aircraftSprites, maxVelocityProvider);
        }

        public ISpriteChooser CreateDummySpriteChooser(Sprite sprite)
        {
            return new DummySpriteChooser(sprite);
        }
    }
}
