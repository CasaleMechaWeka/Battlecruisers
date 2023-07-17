using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies.Requests
{
    public class PvPOffensiveRequest : IPvPOffensiveRequest
    {
        public PvPOffensiveType Type { get; }
        public PvPOffensiveFocus Focus { get; }
        public int NumOfSlotsToUse { get; set; }

        public PvPOffensiveRequest(PvPOffensiveType type, PvPOffensiveFocus focus)
        {
            Type = type;
            Focus = focus;
            NumOfSlotsToUse = 0;
        }

        public PvPOffensiveRequest(IPvPOffensiveRequest requestToCopy)
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
            PvPOffensiveRequest other = obj as PvPOffensiveRequest;
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
