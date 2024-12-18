using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers
{
    public interface IPvPSpriteChooser
    {
        (ISpriteWrapper, int) ChooseSprite(Vector2 velocity);
        ISpriteWrapper ChooseSprite(int index);
    }
}
