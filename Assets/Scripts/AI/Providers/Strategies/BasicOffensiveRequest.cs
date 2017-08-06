namespace BattleCruisers.AI.Providers.Strategies
{
    public class BasicOffensiveRequest : IBasicOffensiveRequest
	{
		public OffensiveType Type { get; private set; }
		public OffensiveFocus Focus { get; private set; }

		public BasicOffensiveRequest(OffensiveType type, OffensiveFocus focus)
		{
			this.Type = type;
			this.Focus = focus;
		}
	}
}
