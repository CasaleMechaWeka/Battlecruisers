using BattleCruisers.Utils;

namespace BattleCruisers.Data.Static.Strategies.Requests
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

        public OffensiveRequest(IOffensiveRequest requestToCopy)
        {
            Type = requestToCopy.Type;
            Focus = requestToCopy.Focus;
            NumOfSlotsToUse = requestToCopy.NumOfSlotsToUse;
        }

        public override string ToString()
        {
            return base.ToString() + "  Type: " + Type + "  Focus: " + Focus + " SlotNum: " + NumOfSlotsToUse;
        }

        public override bool Equals(object obj)
        {
            OffensiveRequest other = obj as OffensiveRequest;
            return
                other != null
                && other.Type == Type
                && other.Focus == Focus
                && other.NumOfSlotsToUse == NumOfSlotsToUse;
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(Type, Focus, NumOfSlotsToUse);
        }
    }
}
