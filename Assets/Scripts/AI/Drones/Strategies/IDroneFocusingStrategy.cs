namespace BattleCruisers.AI.Drones.Strategies
{
    public interface IDroneFocusingStrategy
    {
		bool EvaluateWhenBuildingStarted { get; }
		bool EvaluateWhenUnitStarted { get; }
        bool ForceInProgressBuildingToFocused { get; }
    }
}
