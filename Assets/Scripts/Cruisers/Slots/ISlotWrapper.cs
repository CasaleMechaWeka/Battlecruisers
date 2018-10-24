using BattleCruisers.Buildables.Buildings;
using System.Collections.ObjectModel;

namespace BattleCruisers.Cruisers.Slots
{
    public interface ISlotWrapper
    {
        ReadOnlyCollection<ISlot> GetFreeSlots(SlotType slotType);
        bool IsSlotAvailable(SlotType slotType);
        int GetSlotCount(SlotType slotType);
        // FELIX  Replace parameters with ISlotSpecification :P
		ISlot GetFreeSlot(SlotType slotType, BuildingFunction buildingFunction, bool preferFromFront = true);
        void HighlightAvailableSlots(SlotType slotType);
        void UnhighlightSlots();
        void HighlightBuildingSlot(IBuilding building);
	}
}
