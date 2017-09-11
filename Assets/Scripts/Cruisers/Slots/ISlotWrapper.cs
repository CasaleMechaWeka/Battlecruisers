using System.Collections.ObjectModel;

namespace BattleCruisers.Cruisers.Slots
{
    public interface ISlotWrapper
	{
        ReadOnlyCollection<ISlot> Slots { get; }

        bool IsSlotAvailable(SlotType slotType);
        int GetSlotCount(SlotType slotType);
		ISlot GetFreeSlot(SlotType slotType, bool preferFromFront = true);
        void ShowAllSlots();
        void HideAllSlots();
        void HighlightAvailableSlots(SlotType slotType);
        void UnhighlightSlots();
	}
}
