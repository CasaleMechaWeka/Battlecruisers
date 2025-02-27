using BattleCruisers.Cruisers.Fog;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Fog
{
    public class PvPFogVisibilityDecider : IFogVisibilityDecider
    {
        public bool ShouldFogBeVisible(int numOfFriendlyStealthGenerators, int numOfEnemySpySatellites, int numOfEnemySpyPlanes)
        {
            return numOfFriendlyStealthGenerators != 0 && numOfEnemySpySatellites == 0 && numOfEnemySpyPlanes == 0;
        }
    }
}