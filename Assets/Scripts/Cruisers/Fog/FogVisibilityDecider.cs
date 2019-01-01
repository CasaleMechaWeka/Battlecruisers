namespace BattleCruisers.Cruisers.Fog
{
    public class FogVisibilityDecider : IFogVisibilityDecider
    {
        public bool ShouldFogBeVisible(int numOfFriendlyStealthGenerators, int numOfEnemySpySatellites)
        {
            return numOfFriendlyStealthGenerators != 0 && numOfEnemySpySatellites == 0;
        }
    }
}