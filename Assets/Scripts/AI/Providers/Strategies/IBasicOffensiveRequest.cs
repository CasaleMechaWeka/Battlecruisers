namespace BattleCruisers.AI.Providers.Strategies
{
    public enum OffensiveType
	{
		Air, Naval, Buildings, Ultras
	}

	public enum OffensiveFocus
	{
		Low, High
	}

	public interface IBasicOffensiveRequest
	{
		OffensiveType Type { get; }
		OffensiveFocus Focus { get; }
	}
}
