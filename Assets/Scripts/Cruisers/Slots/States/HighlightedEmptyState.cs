using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Cruisers.Slots.States
{
    public class HighlightedEmptyState : SlotState
    {
        private readonly ICruiser _parentCruiser;
        private readonly ISlot _parentSlot;

        private static Color HIGHLIGHTED_EMPTY_COLOUR = Color.green;

        public HighlightedEmptyState(ICruiser parentCruiser, ISlot parentSlot)
            : base(HIGHLIGHTED_EMPTY_COLOUR)
        {
            Helper.AssertIsNotNull(parentCruiser, parentSlot);

            _parentCruiser = parentCruiser;
            _parentSlot = parentSlot;
        }

        public override void OnClick()
        {
            _parentCruiser.ConstructSelectedBuilding(_parentSlot);
        }
    }
}
