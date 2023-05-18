using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Sprites;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.UI;
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

        public async Task<IPvPSpriteChooser> CreateBomberSpriteChooserAsync(IPvPVelocityProvider maxVelocityProvider)
        {
            IList<IPvPSpriteWrapper> bomberSprites = await _spriteProvider.GetBomberSpritesAsync();
            return new PvPSpriteChooser(_assignerFactory, bomberSprites, maxVelocityProvider);
        }

        public async Task<IPvPSpriteChooser> CreateFighterSpriteChooserAsync(IPvPVelocityProvider maxVelocityProvider)
        {
            IList<IPvPSpriteWrapper> fighterSprites = await _spriteProvider.GetFighterSpritesAsync();
            return new PvPSpriteChooser(_assignerFactory, fighterSprites, maxVelocityProvider);
        }

        public async Task<IPvPSpriteChooser> CreateGunshipSpriteChooserAsync(IPvPVelocityProvider maxVelocityProvider)
        {
            IList<IPvPSpriteWrapper> gunshipSprites = await _spriteProvider.GetGunshipSpritesAsync();
            return new PvPSpriteChooser(_assignerFactory, gunshipSprites, maxVelocityProvider);
        }

        public async Task<IPvPSpriteChooser> CreateSteamCopterSpriteChooserAsync(IPvPVelocityProvider maxVelocityProvider)
        {
            IList<IPvPSpriteWrapper> copterSprites = await _spriteProvider.GetSteamCopterSpritesAsync();
            return new PvPSpriteChooser(_assignerFactory, copterSprites, maxVelocityProvider);
        }

        public IPvPSpriteChooser CreateDummySpriteChooser(Sprite sprite)
        {
            return new PvPDummySpriteChooser(sprite);
        }
    }
}
