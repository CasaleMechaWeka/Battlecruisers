namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots
{
    public class PvPFreeSlotFilter : IPvPSlotFilter
    {
        public bool IsMatch(IPvPSlot slot)
        {
            return slot.IsFree;
        }
    }
}
