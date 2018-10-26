using BattleCruisers.Buildables.Buildings;
using System.Collections.ObjectModel;

namespace BattleCruisers.Cruisers.Slots
{
    // FELIX  Delete :)
    public interface ISlotWrapper
    {
        ReadOnlyCollection<ISlot> GetFreeSlots(SlotType slotType);
        int GetSlotCount(SlotType slotType);
        bool IsSlotAvailable(SlotSpecification slotSpecification);
		ISlot GetFreeSlot(SlotSpecification slotSpecification);
        void HighlightAvailableSlots(SlotType slotType);
        void UnhighlightSlots();
        void HighlightBuildingSlot(IBuilding building);
	}
}
