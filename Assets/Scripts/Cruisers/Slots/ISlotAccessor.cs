using System.Collections.ObjectModel;

namespace BattleCruisers.Cruisers.Slots
{
    public interface ISlotAccessor
    {
        ReadOnlyCollection<ISlot> GetSlots(SlotType slotType);
        ReadOnlyCollection<ISlot> GetFreeSlots(SlotType slotType);
        int GetSlotCount(SlotType slotType);
        bool IsSlotAvailable(SlotSpecification slotSpecification);
		ISlot GetFreeSlot(SlotSpecification slotSpecification);
	}
}
