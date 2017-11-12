using BattleCruisers.Movement.Velocity.Providers;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public interface ISpriteChooserFactory
    {
        ISpriteChooser CreateBomberSpriteChooser(IVelocityProvider maxVelocityProvider);
        ISpriteChooser CreateDummySpriteChooser(Sprite sprite);
    }
}
