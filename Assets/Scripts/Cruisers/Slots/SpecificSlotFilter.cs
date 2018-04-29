namespace BattleCruisers.Cruisers.Slots
{
    public class SpecificSlotFilter : ISlotFilter, ISlotPermitter
    {
        public ISlot PermittedSlot { private get; set; }

        public bool IsMatch(ISlot slot)
        {
            return ReferenceEquals(PermittedSlot, slot);
        }
    }
}
