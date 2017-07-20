namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public interface IFireIntervalManager
	{
		/// <summary>
		/// Automatically starts the next interval if the current interval is up.
		/// So two consecutive calls to could return "true" and "false" respectively.
		/// </summary>
		/// <returns><c>true</c> if the fire interval is up; otherwise, <c>false</c>.</returns>
		bool IsIntervalUp();
	}
}
