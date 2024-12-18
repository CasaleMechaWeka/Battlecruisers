using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;

using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers
{
    public class PvPDummySpriteChooser : IPvPSpriteChooser
    {
        private ISpriteWrapper _sprite;

        public PvPDummySpriteChooser(Sprite sprite)
        {
            Assert.IsNotNull(sprite);
            _sprite = new SpriteWrapper(sprite);
        }

        public (ISpriteWrapper, int) ChooseSprite(Vector2 velocity)
        {
            return (_sprite, 0);
        }
        public ISpriteWrapper ChooseSprite(int index)
        {
            return _sprite;
        }
    }
}
