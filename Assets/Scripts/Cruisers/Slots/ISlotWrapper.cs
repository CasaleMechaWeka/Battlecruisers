using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Utils;

namespace BattleCruisers.Cruisers.Slots
{
    public interface ISlotWrapper : IManagedDisposable
    {
        ReadOnlyCollection<ISlot> GetFreeSlots(SlotType slotType);
        bool IsSlotAvailable(SlotType slotType);
        int GetSlotCount(SlotType slotType);
		ISlot GetFreeSlot(SlotType slotType, bool preferFromFront = true);
        void HighlightAvailableSlots(SlotType slotType);
        void UnhighlightSlots();
        void HighlightBuildingSlot(IBuilding building);
	}
}
