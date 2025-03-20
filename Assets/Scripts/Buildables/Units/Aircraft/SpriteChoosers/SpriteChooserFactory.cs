using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public class SpriteChooserFactory : ISpriteChooserFactory
    {
        private readonly IAssignerFactory _assignerFactory;
        private readonly ISpriteProvider _spriteProvider;

        public SpriteChooserFactory(IAssignerFactory assignerFactory, ISpriteProvider spriteProvider)
        {
            Helper.AssertIsNotNull(assignerFactory, spriteProvider);

            _assignerFactory = assignerFactory;
            _spriteProvider = spriteProvider;
        }

        public async Task<ISpriteChooser> CreateAircraftSpriteChooserAsync(PrefabKeyName prefabKeyName, IVelocityProvider maxVelocityProvider)
        {
            IList<Sprite> aircraftSprites = await _spriteProvider.GetAircraftSpritesAsync(prefabKeyName);
            return new SpriteChooser(_assignerFactory, aircraftSprites, maxVelocityProvider);
        }

        public ISpriteChooser CreateDummySpriteChooser(Sprite sprite)
        {
            return new DummySpriteChooser(sprite);
        }
    }
}
