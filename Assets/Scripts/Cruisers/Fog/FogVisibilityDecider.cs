namespace BattleCruisers.Cruisers.Fog
{
    // FELIX  Test :)
    public class FogVisibilityDecider : IFogVisibilityDecider
    {
        public bool ShouldFogBeVisible(int numOfFriendlyStealthGenerators, int numOfEnemySpySatellites)
        {
            return numOfFriendlyStealthGenerators != 0 && numOfEnemySpySatellites == 0;
        }
    }
}