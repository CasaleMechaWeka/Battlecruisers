using BattleCruisers.Utils.UIWrappers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public class DummySpriteChooser : ISpriteChooser
    {
        private ISpriteWrapper _sprite;

        public DummySpriteChooser(Sprite sprite)
        {
            Assert.IsNotNull(sprite);
            _sprite = new SpriteWrapper(sprite);
        }

        public ISpriteWrapper ChooseSprite(Vector2 velocity)
        {
            return _sprite;
        }
    }
}
