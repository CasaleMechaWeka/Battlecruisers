namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    public interface ISpriteChooserFactory
    {
        ISpriteChooser CreateBomberSpriteChooser(float maxVelocityInMPerS);
    }
}
