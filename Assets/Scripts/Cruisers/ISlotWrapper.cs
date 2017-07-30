namespace BattleCruisers.Cruisers
{
    public enum SlotLocation
    {
        Front, Middle, Rear
    }

    public interface ISlotWrapper
	{
        bool IsSlotAvailable(SlotType slotType);
        int GetSlotCount(SlotType slotType);
        // FELIX  Combine methods?
		ISlot GetFreeSlot(SlotType slotType);
        ISlot GetFreeSlot(SlotType slotType, SlotLocation preferredLocation);
        void ShowAllSlots();
        void HideAllSlots();
        void HighlightAvailableSlots(SlotType slotType);
        void UnhighlightSlots();
	}
}
