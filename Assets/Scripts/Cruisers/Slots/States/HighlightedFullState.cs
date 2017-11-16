using UnityEngine;

namespace BattleCruisers.Cruisers.Slots.States
{
    public class HighlightedFullState : SlotState
    {
        private static Color HIGHLIGHTED_FULL_COLOUR = Color.green;

        public HighlightedFullState()
            : base(HIGHLIGHTED_FULL_COLOUR)
        {
        }
    }
}
