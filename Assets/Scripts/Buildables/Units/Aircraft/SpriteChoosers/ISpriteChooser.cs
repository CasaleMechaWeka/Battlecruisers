using UnityEngine;

namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public interface ISpriteChooser
    {
        Sprite ChooseSprite(Vector2 velocity);
    }
}
