namespace BattleCruisers.AI.Drones.Strategies
{

    /// <summary>
    /// Evaluates both when buildings and units start getting built.
    /// 
    /// Does not force in progress buildings to the focused state, avoiding
    /// factory starvation (as long as the factory unit can be afforded
    /// as well as having the in progress building as active).
    /// 
    /// Hence good to use with a conservative IUnitChooser.
    /// </summary>
    public class ResponsiveStrategy : IDroneFocusingStrategy
	{
        public bool EvaluateWhenBuildingStarted { get; }
		public bool EvaluateWhenUnitStarted { get; }
		public bool ForceInProgressBuildingToFocused { get; }

        public ResponsiveStrategy()
        {
            EvaluateWhenBuildingStarted = true;
            EvaluateWhenUnitStarted = true;
            ForceInProgressBuildingToFocused = false;
        }
	}
}
