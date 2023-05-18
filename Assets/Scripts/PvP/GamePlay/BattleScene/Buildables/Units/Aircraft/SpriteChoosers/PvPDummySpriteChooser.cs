using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers
{
    public class PvPDummySpriteChooser : IPvPSpriteChooser
    {
        private IPvPSpriteWrapper _sprite;

        public PvPDummySpriteChooser(Sprite sprite)
        {
            Assert.IsNotNull(sprite);
            _sprite = new PvPSpriteWrapper(sprite);
        }

        public IPvPSpriteWrapper ChooseSprite(Vector2 velocity)
        {
            return _sprite;
        }
    }
}
