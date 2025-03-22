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
        public async Task<ISpriteChooser> CreateAircraftSpriteChooserAsync(PrefabKeyName prefabKeyName, IVelocityProvider maxVelocityProvider)
        {
            IList<Sprite> aircraftSprites = await SpriteProvider.GetAircraftSpritesAsync(prefabKeyName);
            return new SpriteChooser(aircraftSprites, maxVelocityProvider);
        }

        public ISpriteChooser CreateDummySpriteChooser(Sprite sprite)
        {
            return new DummySpriteChooser(sprite);
        }
    }
}
