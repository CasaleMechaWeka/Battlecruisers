namespace BattleCruisers.Cruisers.Slots
{
    // FELIX  Test
    public class SpecificSlotFilter : ISlotFilter, ISlotPermitter
    {
        public ISlot PermittedSlot { private get; set; }

        public bool IsMatch(ISlot slot)
        {
            return ReferenceEquals(PermittedSlot, slot);
        }
    }
}
