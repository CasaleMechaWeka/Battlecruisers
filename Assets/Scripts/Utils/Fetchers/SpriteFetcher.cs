using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Utils.Fetchers
{
    public class SpriteFetcher : ISpriteFetcher
	{
        public IList<ISpriteWrapper> GetMultiSprites(string spritePath)
		{
            Sprite[] sprites = Resources.LoadAll<Sprite>(spritePath);

            return
                sprites
                    .Select(sprite => (ISpriteWrapper)new SpriteWrapper(sprite))
                    .ToList();
		}
	}
}
