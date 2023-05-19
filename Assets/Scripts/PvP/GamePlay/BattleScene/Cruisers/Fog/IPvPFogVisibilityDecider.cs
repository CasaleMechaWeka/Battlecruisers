namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Fog
{
    public interface IPvPFogVisibilityDecider
    {
        bool ShouldFogBeVisible(int numOfFriendlyStealthGenerators, int numOfEnemySpySatellites);
    }
}