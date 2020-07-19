using BattleCruisers.Buildables.Buildings;

namespace BattleCruisers.Cruisers.Slots
{
    public interface ISlotHighlighter
    {
        /// <returns>True if one or more slots was highlighted, false if there are no available slots.</returns>
        bool HighlightAvailableSlots(ISlotSpecification slotSpecification);

        /// <summary>
        /// Highlights all slots matching the given specification, regardless of whether
        /// they are free or not.
        /// </summary>
        void HighlightSlots(ISlotSpecification slotSpecification);

        void UnhighlightSlots();
        void HighlightBuildingSlot(IBuilding building);
	}
}
