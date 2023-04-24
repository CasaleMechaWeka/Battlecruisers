using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildable.Buildings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Slots
{
    public interface IPvPSlotAccessor
    {
        bool IsSlotAvailable(IPvPSlotSpecification slotSpecification);
        bool IsSlotAvailableForPlayer(IPvPSlotSpecification slotSpecification);

        /// <returns>
        /// If looking for anti-ship slots, only returns slot placed well for anti-ship
        /// turrets (first two slots).  This is to help noob users so they don't build
        /// anti-ship turrets at the back of the cruiser, which they seem to have a 
        /// tendency to do.
        /// 
        /// If looking for non anti-ship slots returns all slots of that SlotType
        /// (eg: all deck slots).
        /// </returns>
        ReadOnlyCollection<IPvPSlot> GetSlots(IPvPSlotSpecification slotSpecification);

        IList<IPvPSlot> GetFreeSlots(SlotType slotType);
        IPvPSlot GetFreeSlot(IPvPSlotSpecification slotSpecification);

        /// <returns>
        /// The slot that currently contains the given building, or null if no such slot exists.
        /// </returns>
        IPvPSlot GetSlot(IPvPBuilding building);

        int GetSlotCount(SlotType slotType);
    }
}

