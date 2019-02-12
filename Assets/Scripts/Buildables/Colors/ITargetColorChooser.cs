using UnityEngine;

namespace BattleCruisers.Buildables.Colors
{
    public enum TargetState
    {
        Default,
        Selected,
        Targetted
    }

    public interface ITargetColorChooser
    {
        Color ChooseColour(TargetState targetState);
    }
}