namespace BattleCruisers.Cruisers.Slots
{
    // FELIX  Test :P
    public class FreeSlotFilter : ISlotFilter
    {
        public bool IsMatch(ISlot slot)
        {
            return slot.IsFree;
        }
    }
}
