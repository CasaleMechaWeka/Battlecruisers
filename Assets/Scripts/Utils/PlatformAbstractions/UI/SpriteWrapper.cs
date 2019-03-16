using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    public class SpriteWrapper : ISpriteWrapper
    {
        public Sprite Sprite { get; }

        public SpriteWrapper(Sprite sprite)
        {
            Assert.IsNotNull(sprite);
            Sprite = sprite;
        }
    }
}
