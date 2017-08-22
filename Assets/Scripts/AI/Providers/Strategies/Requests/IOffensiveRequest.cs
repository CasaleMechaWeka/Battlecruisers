namespace BattleCruisers.AI.Providers.Strategies.Requests
{
	public enum OffensiveType
	{
		Air, Naval, Buildings, Ultras
	}

	public enum OffensiveFocus
	{
		Low, High
	}

    // FELIX  Move up one namespace
	public interface IOffensiveRequest
	{
		OffensiveType Type { get; }
		OffensiveFocus Focus { get; }
		int NumOfSlotsToUse { get; set; }
	}
}
