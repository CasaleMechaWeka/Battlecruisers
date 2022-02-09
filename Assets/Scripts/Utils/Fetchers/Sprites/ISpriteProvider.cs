using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BattleCruisers.Utils.Fetchers.Sprites
{
    public interface ISpriteProvider
    {
        Task<IList<ISpriteWrapper>> GetBomberSpritesAsync();
        Task<IList<ISpriteWrapper>> GetFighterSpritesAsync();
        Task<IList<ISpriteWrapper>> GetGunshipSpritesAsync();
        Task<IList<ISpriteWrapper>> GetSteamCopterSpritesAsync();
    }
}
