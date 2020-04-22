using BattleCruisers.Buildables.Buildings;
using System.Collections.ObjectModel;

namespace BattleCruisers.Cruisers.Slots
{
    public interface ISlotAccessor
    {
        bool IsSlotAvailable(SlotSpecification slotSpecification);

        /// <returns>
        /// If looking for anti-ship slots, only returns slot placed well for anti-ship
        /// turrets (first two slots).  This is to help noob users so they don't build
        /// anti-ship turrets at the back of the cruiser, which they seem to have a 
        /// tendency to do.
        /// 
        /// If looking for non anti-ship slots returns all slots of that SlotType
        /// (eg: all deck slots).
        /// </returns>
        ReadOnlyCollection<ISlot> GetSlots(SlotSpecification slotSpecification);

        ISlot GetFreeSlot(SlotSpecification slotSpecification);
		
        /// <returns>
        /// The slot that currently contains the given building, or null if no such slot exists.
        /// </returns>
        ISlot GetSlot(IBuilding building);
        
        int GetSlotCount(SlotType slotType);
	}
}
