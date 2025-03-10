using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers
{
    public class PvPSpriteChooserFactory : IPvPSpriteChooserFactory
    {
        private readonly IAssignerFactory _assignerFactory;
        private readonly ISpriteProvider _spriteProvider;

        public PvPSpriteChooserFactory(IAssignerFactory assignerFactory, ISpriteProvider spriteProvider)
        {
            PvPHelper.AssertIsNotNull(assignerFactory, spriteProvider);

            _assignerFactory = assignerFactory;
            _spriteProvider = spriteProvider;
        }

        public async Task<IPvPSpriteChooser> CreateAircraftSpriteChooserAsync(PrefabKeyName prefabKeyName, IVelocityProvider maxVelocityProvider)
        {
            IList<ISpriteWrapper> aircraftSprites = await _spriteProvider.GetAircraftSpritesAsync(prefabKeyName);
            return new PvPSpriteChooser(_assignerFactory, aircraftSprites, maxVelocityProvider);
        }

        public IPvPSpriteChooser CreateDummySpriteChooser(Sprite sprite)
        {
            return new PvPDummySpriteChooser(sprite);
        }
    }
}
