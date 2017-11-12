using System.Collections.Generic;
using BattleCruisers.Fetchers;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.UIWrappers;

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

        public ISpriteChooser CreateBomberSpriteChooser(IVelocityProvider maxVelocityProvider)
        {
            IList<ISpriteWrapper> bomberSprites = _spriteProvider.GetBomberSprites();
            return new SpriteChooser(_assignerFactory, bomberSprites, maxVelocityProvider);
        }
    }
}
