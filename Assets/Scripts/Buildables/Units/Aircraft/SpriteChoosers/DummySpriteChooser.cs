using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public class DummySpriteChooser : ISpriteChooser
    {
        private Sprite _sprite;

        public DummySpriteChooser(Sprite sprite)
        {
            Assert.IsNotNull(sprite);
            _sprite = sprite;
        }

        public Sprite ChooseSprite(Vector2 velocity)
        {
            return _sprite;
        }
    }
}
