using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.PlatformAbstractions.UI;
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

        public async Task<ISpriteChooser> CreateBomberSpriteChooserAsync(IVelocityProvider maxVelocityProvider)
        {
            IList<ISpriteWrapper> bomberSprites = await _spriteProvider.GetBomberSpritesAsync();
            return new SpriteChooser(_assignerFactory, bomberSprites, maxVelocityProvider);
        }
		
		public async Task<ISpriteChooser> CreateFighterSpriteChooserAsync(IVelocityProvider maxVelocityProvider)
		{
            IList<ISpriteWrapper> fighterSprites = await _spriteProvider.GetFighterSpritesAsync();
            return new SpriteChooser(_assignerFactory, fighterSprites, maxVelocityProvider);
		}

        public async Task<ISpriteChooser> CreateGunshipSpriteChooserAsync(IVelocityProvider maxVelocityProvider)
        {
            IList<ISpriteWrapper> gunshipSprites = await _spriteProvider.GetGunshipSpritesAsync();
            return new SpriteChooser(_assignerFactory, gunshipSprites, maxVelocityProvider);
        }

        public ISpriteChooser CreateDummySpriteChooser(Sprite sprite)
        {
            return new DummySpriteChooser(sprite);
        }
    }
}
