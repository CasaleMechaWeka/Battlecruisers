using System.Collections.Generic;

namespace BattleCruisers.Cruisers.Slots
{
    public class SpecificSlotsFilter : ISlotFilter, ISlotPermitter
    {
        public IList<ISlot> PermittedSlots { private get; set; }

        public bool IsMatch(ISlot slot)
        {
            return 
                PermittedSlots != null
                && PermittedSlots.Contains(slot);
        }
    }
}
