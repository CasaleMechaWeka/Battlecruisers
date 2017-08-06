namespace BattleCruisers.AI.Providers
{
    public class OffensiveRequest
    {
        public OffensiveType Type { get; private set; }
        public OffensiveFocus Focus { get; private set; }

        public OffensiveRequest(OffensiveType type, OffensiveFocus focus)
        {
            this.Type = type;
            this.Focus = focus;
        }
    }
}
