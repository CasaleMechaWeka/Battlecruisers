using UnityEngine;

// FELIX  Delete all :)
namespace BattleCruisers.Cruisers.Slots.States
{
    public interface ISlotState
    {
        Color Colour { get; }

        void OnClick();
    }
}
