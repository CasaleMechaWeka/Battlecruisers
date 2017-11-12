using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.UIWrappers
{
    public class SpriteWrapper : ISpriteWrapper
    {
        public Sprite Sprite { get; private set; }

        public SpriteWrapper(Sprite sprite)
        {
            Assert.IsNotNull(sprite);
            Sprite = sprite;
        }
    }
}
