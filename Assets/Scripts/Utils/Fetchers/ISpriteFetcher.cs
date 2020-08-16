using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BattleCruisers.Utils.Fetchers
{
    public interface ISpriteFetcher
	{
        Task<ISpriteWrapper> GetSpriteAsync(string spritePath);
        Task<IList<ISpriteWrapper>> GetMultiSpritesAsync(string spritePath);
	}
}
