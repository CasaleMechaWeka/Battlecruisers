using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots
{
    public interface IPvPSlotHighlighter
    {
        /// <returns>True if one or more slots was highlighted, false if there are no available slots.</returns>
        bool HighlightAvailableSlots(ISlotSpecification slotSpecification);

        /// <summary>
        /// Highlights all slots matching the given specification, regardless of whether
        /// they are free or not.
        /// </summary>
        void HighlightSlots(ISlotSpecification slotSpecification);

        void UnhighlightSlots();
        void HighlightBuildingSlot(IPvPBuilding building);
        void HighlightAvailableSlotsCurrent();
    }
}
