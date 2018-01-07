using System;
using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Utils.UIWrappers;
using UnityEngine;

namespace BattleCruisers.Utils.Fetchers
{
    public class SpriteFetcher : ISpriteFetcher
	{
        public ISpriteWrapper GetSprite(string spritePath)
        {
            Sprite sprite = Resources.Load<Sprite>(spritePath);

            if (sprite == null)
            {
                throw new ArgumentException("Invalid sprite path: " + spritePath);
            }

            return new SpriteWrapper(sprite);
        }
		
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
