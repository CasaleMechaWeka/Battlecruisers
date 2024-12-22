using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.UI;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers
{
    public interface IPvPSpriteChooser
    {
        (IPvPSpriteWrapper, int) ChooseSprite(Vector2 velocity);
        IPvPSpriteWrapper ChooseSprite(int index);
    }
}
