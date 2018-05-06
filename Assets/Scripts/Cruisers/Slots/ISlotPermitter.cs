using System.Collections.Generic;

namespace BattleCruisers.Cruisers.Slots
{
    public interface ISlotPermitter
    {
        IList<ISlot> PermittedSlots { set; }
    }
}
