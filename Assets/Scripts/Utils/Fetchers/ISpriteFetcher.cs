using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BattleCruisers.Utils.Fetchers
{
    public interface ISpriteFetcher
	{
        Task<IList<ISpriteWrapper>> GetMultiSpritesAsync(string spritePath);
	}
}
