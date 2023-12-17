namespace BattleCruisers.Cruisers.Fog
{
    public class FogVisibilityDecider : IFogVisibilityDecider
    {
        public bool ShouldFogBeVisible(int numOfFriendlyStealthGenerators, int numOfEnemySpySatellites, int numOfEnemySpyPlanes)
        {
            return numOfFriendlyStealthGenerators != 0 && numOfEnemySpySatellites == 0 && numOfEnemySpyPlanes == 0;
        }
    }
}