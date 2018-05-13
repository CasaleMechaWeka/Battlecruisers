using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.Cruisers.Slots
{
    public interface ISlotWrapper
	{
        ReadOnlyCollection<ISlot> GetFreeSlots(SlotType slotType);
        bool IsSlotAvailable(SlotType slotType);
        int GetSlotCount(SlotType slotType);
		ISlot GetFreeSlot(SlotType slotType, bool preferFromFront = true);
        void ShowAllSlots();
        void HideAllSlots();
        void HighlightAvailableSlots(SlotType slotType);
        void UnhighlightSlots();
        void HighlightBuildingSlot(IBuilding building);
	}
}
