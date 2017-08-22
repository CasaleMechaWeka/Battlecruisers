namespace BattleCruisers.AI.Providers.Strategies.Requests
{
    public class OffensiveRequest : IOffensiveRequest
    {
		public OffensiveType Type { get; private set; }
		public OffensiveFocus Focus { get; private set; }
		public int NumOfSlotsToUse { get; set; }

        public OffensiveRequest(OffensiveType type, OffensiveFocus focus)
        {
            Type = type;
            Focus = focus;
            NumOfSlotsToUse = 0;
        }
    }
}
