using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.UI
{
    public class PvPSpriteWrapper : IPvPSpriteWrapper
    {
        public Sprite Sprite { get; }

        public PvPSpriteWrapper(Sprite sprite)
        {
            Assert.IsNotNull(sprite);
            Sprite = sprite;
        }
    }
}
