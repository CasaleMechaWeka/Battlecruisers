namespace BattleCruisers.Cruisers
{
    public interface ISlotWrapper
	{
        bool IsSlotAvailable(SlotType slotType);
        void ShowAllSlots();
        void HideAllSlots();
        void HighlightAvailableSlots(SlotType slotType);
        void UnhighlightSlots();
        ISlot GetFreeSlot(SlotType slotType);
        int GetSlotCount(SlotType slotType);
	}
}
