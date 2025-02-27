using BattleCruisers.Data.Static.Strategies.Requests;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies.Requests
{
    public class PvPOffensiveRequest : IOffensiveRequest
    {
        public OffensiveType Type { get; }
        public OffensiveFocus Focus { get; }
        public int NumOfSlotsToUse { get; set; }

        public PvPOffensiveRequest(OffensiveType type, OffensiveFocus focus)
        {
            Type = type;
            Focus = focus;
            NumOfSlotsToUse = 0;
        }

        public PvPOffensiveRequest(IOffensiveRequest requestToCopy)
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
