using UnityEngine;

namespace BattleCruisers.Cruisers.Slots.States
{
    public class DefaultState : SlotState
    {
        private static Color DEFAULT_COLOUR = Color.yellow;

        public DefaultState() 
            : base(DEFAULT_COLOUR)
        {
        }
    }
}
