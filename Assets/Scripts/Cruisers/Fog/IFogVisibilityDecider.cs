namespace BattleCruisers.Cruisers.Fog
{
    public interface IFogVisibilityDecider
    {
        bool ShouldFogBeVisible(int numOfFriendlyStealthGenerators, int numOfEnemySpySatellites);
    }
}