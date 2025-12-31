namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress
{
    public interface IPvPBuildProgressCalculator
    {
        float CalculateBuildProgressInDroneS(IPvPBuildable buildableUnderConstruction, float deltaTime);
    }
}
