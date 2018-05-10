namespace BattleCruisers.Buildables.BuildProgress
{
    public interface IBuildProgressCalculator
    {
        float CalculateBuildProgressInDroneS(IBuildable buildableUnderConstruction, float deltaTime);
    }
}