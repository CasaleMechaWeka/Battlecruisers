namespace BattleCruisers.Data.Static.Strategies.Requests
{
	public enum OffensiveType
	{
		Air, Naval, Buildings, Ultras
	}

	public enum OffensiveFocus
	{
		Low, High
	}

	public interface IOffensiveRequest
	{
		OffensiveType Type { get; }
		OffensiveFocus Focus { get; }
		int NumOfSlotsToUse { get; set; }
	}
}
