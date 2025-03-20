using UnityEngine;

using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers
{
    public class PvPDummySpriteChooser : IPvPSpriteChooser
    {
        private Sprite _sprite;

        public PvPDummySpriteChooser(Sprite sprite)
        {
            Assert.IsNotNull(sprite);
            _sprite = sprite;
        }

        public (Sprite, int) ChooseSprite(Vector2 velocity)
        {
            return (_sprite, 0);
        }
        public Sprite ChooseSprite(int index)
        {
            return _sprite;
        }
    }
}
