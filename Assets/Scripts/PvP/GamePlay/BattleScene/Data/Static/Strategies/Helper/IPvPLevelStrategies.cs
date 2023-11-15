namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies.Helper
{
    public interface IPvPLevelStrategies
    {
        IPvPStrategy GetAdaptiveStrategy(int levelNum);
        IPvPStrategy GetBasicStrategy(int levelNum);
    }
}
