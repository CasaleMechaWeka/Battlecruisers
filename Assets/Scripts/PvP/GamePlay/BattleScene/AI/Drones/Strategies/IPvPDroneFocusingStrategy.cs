namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones.Strategies
{
    public interface IPvPDroneFocusingStrategy
    {
        bool EvaluateWhenBuildingStarted { get; }
        bool EvaluateWhenUnitStarted { get; }
        bool ForceInProgressBuildingToFocused { get; }
    }
}
