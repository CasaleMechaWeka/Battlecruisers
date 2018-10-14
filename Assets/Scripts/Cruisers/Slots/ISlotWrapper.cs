using BattleCruisers.Buildables.Buildings;
using System.Collections.ObjectModel;

namespace BattleCruisers.Cruisers.Slots
{
    public interface ISlotWrapper
    {
        ReadOnlyCollection<ISlot> GetFreeSlots(SlotType slotType);
        bool IsSlotAvailable(SlotType slotType);
        int GetSlotCount(SlotType slotType);
		ISlot GetFreeSlot(SlotType slotType, bool preferFromFront = true);
        void HighlightAvailableSlots(SlotType slotType);
        void UnhighlightSlots();
        // FELIX  Replace with highlighting building instead (ie, make building red 
        // instead of black, like in Peter's mock ups :) )
        void HighlightBuildingSlot(IBuilding building);
	}
}
