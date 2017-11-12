using BattleCruisers.Utils.UIWrappers;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public interface ISpriteChooser
    {
        ISpriteWrapper ChooseSprite(Vector2 velocity);
    }
}
