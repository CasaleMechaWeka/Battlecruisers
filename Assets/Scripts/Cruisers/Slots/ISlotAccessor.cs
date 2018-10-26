using BattleCruisers.Buildables.Buildings;
using System.Collections.ObjectModel;

namespace BattleCruisers.Cruisers.Slots
{
    public interface ISlotAccessor
    {
        bool IsSlotAvailable(SlotSpecification slotSpecification);
        ReadOnlyCollection<ISlot> GetSlots(SlotType slotType);
        ReadOnlyCollection<ISlot> GetFreeSlots(SlotType slotType);
		ISlot GetFreeSlot(SlotSpecification slotSpecification);
        ISlot GetSlot(IBuilding building);
        int GetSlotCount(SlotType slotType);
	}
}
