using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.Cruisers.Slots
{
    public interface ISlotHighlighter
    {
        /// <returns>True if one or more slots was highlighted, false if there are no available slots.</returns>
        bool HighlightAvailableSlots(SlotType slotType);
        void UnhighlightSlots();
        void HighlightBuildingSlot(IBuilding building);
	}
}
