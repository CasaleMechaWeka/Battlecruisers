using BattleCruisers.Movement.Velocity.Providers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers
{
    public class PvPSpriteChooserFactory : IPvPSpriteChooserFactory
    {
        public async Task<IPvPSpriteChooser> CreateAircraftSpriteChooserAsync(PrefabKeyName prefabKeyName, IVelocityProvider maxVelocityProvider)
        {
            IList<Sprite> aircraftSprites = await SpriteProvider.GetAircraftSpritesAsync(prefabKeyName);
            return new PvPSpriteChooser(aircraftSprites, maxVelocityProvider);
        }

        public IPvPSpriteChooser CreateDummySpriteChooser(Sprite sprite)
        {
            return new PvPDummySpriteChooser(sprite);
        }
    }
}
