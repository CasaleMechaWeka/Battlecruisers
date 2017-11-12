using System.Collections.Generic;
using BattleCruisers.Utils.UIWrappers;

namespace BattleCruisers.Fetchers
{
    public interface ISpriteFetcher
	{
		ISpriteWrapper GetSprite(string spritePath);
        IList<ISpriteWrapper> GetMultiSprites(string spritePath);
	}
}
