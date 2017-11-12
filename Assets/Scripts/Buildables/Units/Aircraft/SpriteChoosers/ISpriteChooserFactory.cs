using BattleCruisers.Movement.Velocity.Providers;

namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public interface ISpriteChooserFactory
    {
        ISpriteChooser CreateBomberSpriteChooser(IVelocityProvider maxVelocityProvider);
    }
}
