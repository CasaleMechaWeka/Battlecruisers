using System.Collections.Generic;
using BattleCruisers.Fetchers;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.UIWrappers;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public class SpriteChooserFactory : ISpriteChooserFactory
    {
        private readonly IAssignerFactory _assignerFactory;
        private readonly ISpriteProvider _spriteProvider;

        private const float BOMBER_ASSIGNER_BASE_CUTOFF = 0.9f;
        private const float FIGHTER_ASSIGNER_BASE_CUTOFF = 0.9f;

        public SpriteChooserFactory(IAssignerFactory assignerFactory, ISpriteProvider spriteProvider)
        {
            Helper.AssertIsNotNull(assignerFactory, spriteProvider);

            _assignerFactory = assignerFactory;
            _spriteProvider = spriteProvider;
        }

        public ISpriteChooser CreateBomberSpriteChooser(IVelocityProvider maxVelocityProvider)
        {
            IList<ISpriteWrapper> bomberSprites = _spriteProvider.GetBomberSprites();
            return new SpriteChooser(_assignerFactory, bomberSprites, maxVelocityProvider, BOMBER_ASSIGNER_BASE_CUTOFF);
        }
		
		public ISpriteChooser CreateFighterSpriteChooser(IVelocityProvider maxVelocityProvider)
		{
            IList<ISpriteWrapper> fighterSprites = _spriteProvider.GetFighterSprites();
            return new SpriteChooser(_assignerFactory, fighterSprites, maxVelocityProvider, FIGHTER_ASSIGNER_BASE_CUTOFF);
		}

        public ISpriteChooser CreateDummySpriteChooser(Sprite sprite)
        {
            return new DummySpriteChooser(sprite);
        }
    }
}
