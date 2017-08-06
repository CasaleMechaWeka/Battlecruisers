namespace BattleCruisers.AI.Providers
{
	public interface IOffensiveRequest
	{
        OffensiveType Type { get; }
        OffensiveFocus Focus { get; }
	}
}
