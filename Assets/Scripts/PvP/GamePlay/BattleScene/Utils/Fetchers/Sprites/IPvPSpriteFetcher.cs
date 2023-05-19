using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Sprites
{
    public interface IPvPSpriteFetcher
    {
        Task<IPvPSpriteWrapper> GetSpriteAsync(string spritePath);
        Task<IList<IPvPSpriteWrapper>> GetMultiSpritesAsync(string spritePath);
    }
}
