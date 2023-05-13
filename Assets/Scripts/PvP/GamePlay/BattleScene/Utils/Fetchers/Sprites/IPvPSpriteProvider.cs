using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Sprites
{
    public interface IPvPSpriteProvider
    {
        Task<IList<IPvPSpriteWrapper>> GetBomberSpritesAsync();
        Task<IList<IPvPSpriteWrapper>> GetFighterSpritesAsync();
        Task<IList<IPvPSpriteWrapper>> GetGunshipSpritesAsync();
        Task<IList<IPvPSpriteWrapper>> GetSteamCopterSpritesAsync();
    }
}
