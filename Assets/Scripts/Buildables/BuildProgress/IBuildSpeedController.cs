namespace BattleCruisers.Buildables.BuildProgress
{
    public enum BuildSpeed
    {
        InfinitelySlow, // Buildables progress but never complete
        Normal,         // Buildables complete as they should
        VeryFast        // Buildables complete very quickly
    }

    public interface IBuildSpeedController
    {
        BuildSpeed BuildSpeed { set; }
    }
}
