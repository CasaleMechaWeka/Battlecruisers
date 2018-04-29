namespace BattleCruisers.Cruisers.Slots
{
    public class FreeSlotFilter : ISlotFilter
    {
        public bool IsMatch(ISlot slot)
        {
            return slot.IsFree;
        }
    }
}
