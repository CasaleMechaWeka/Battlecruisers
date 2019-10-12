using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Collections.Generic;

namespace BattleCruisers.Utils.Fetchers
{
    public interface ISpriteFetcher
	{
        IList<ISpriteWrapper> GetMultiSprites(string spritePath);
	}
}
