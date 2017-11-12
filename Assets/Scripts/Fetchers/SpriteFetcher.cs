using System;
using BattleCruisers.Utils.UIWrappers;
using UnityEngine;

namespace BattleCruisers.Fetchers
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
	}
}
