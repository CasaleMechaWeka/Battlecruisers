using System;
using UnityEngine;

namespace BattleCruisers.Buildables.Colors
{
    // FELIX  Delete?
    public class TargetColorChooser : ITargetColorChooser
    {
        // FELIX  Make colours match orange/red in other parts of game :)
        private static Color DefaultColor = Color.black;
        private static Color SelectedColor = Color.magenta;
        private static Color TargettedColor = Color.red;

        public Color ChooseColour(TargetState targetState)
        {
            switch (targetState)
            {
                case TargetState.Default:
                    return DefaultColor;

                case TargetState.Selected:
                    return SelectedColor;

                case TargetState.Targetted:
                    return TargettedColor;

                default:
                    throw new ArgumentException();
            }
        }
    }
}