using UnityEngine;

namespace BattleCruisers.Cruisers.Slots.States
{
    public abstract class SlotState : ISlotState
    {
        public Color Colour { get; private set; }

        protected SlotState(Color colour)
        {
            Colour = colour;
        }

        public virtual void OnClick() { }
    }
}
