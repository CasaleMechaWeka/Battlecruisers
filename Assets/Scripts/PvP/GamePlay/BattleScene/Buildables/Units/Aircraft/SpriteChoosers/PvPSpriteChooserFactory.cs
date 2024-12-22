using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Sprites;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers
{
    public class PvPSpriteChooserFactory : IPvPSpriteChooserFactory
    {
        private readonly IPvPAssignerFactory _assignerFactory;
        private readonly IPvPSpriteProvider _spriteProvider;

        public PvPSpriteChooserFactory(IPvPAssignerFactory assignerFactory, IPvPSpriteProvider spriteProvider)
        {
            PvPHelper.AssertIsNotNull(assignerFactory, spriteProvider);

            _assignerFactory = assignerFactory;
            _spriteProvider = spriteProvider;
        }

        public async Task<IPvPSpriteChooser> CreateAircraftSpriteChooserAsync(PrefabKeyName prefabKeyName, IPvPVelocityProvider maxVelocityProvider)
        {
            IList<IPvPSpriteWrapper> aircraftSprites = await _spriteProvider.GetAircraftSpritesAsync(prefabKeyName);
            return new PvPSpriteChooser(_assignerFactory, aircraftSprites, maxVelocityProvider);
        }

        public IPvPSpriteChooser CreateDummySpriteChooser(Sprite sprite)
        {
            return new PvPDummySpriteChooser(sprite);
        }
    }
}
