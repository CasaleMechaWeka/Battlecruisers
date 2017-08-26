namespace BattleCruisers.AI.Drones
{
    public interface IDroneFocusingStrategy
    {
		bool EvaluateWhenBuildingStarted { get; }
		bool EvaluateWhenUnitStarted { get; }
        bool ForceInProgressBuildingToFocused { get; }
    }
}
