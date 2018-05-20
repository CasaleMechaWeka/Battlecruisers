using System.Collections.Generic;
using BattleCruisers.Utils.PlatformAbstractions.UI;

namespace BattleCruisers.Utils.Fetchers
{
    public interface ISpriteFetcher
	{
		ISpriteWrapper GetSprite(string spritePath);
        IList<ISpriteWrapper> GetMultiSprites(string spritePath);
	}
}
