using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft.SpriteChoosers
{
    public interface IPvPSpriteChooser
    {
        (Sprite, int) ChooseSprite(Vector2 velocity);
        Sprite ChooseSprite(int index);
    }
}
