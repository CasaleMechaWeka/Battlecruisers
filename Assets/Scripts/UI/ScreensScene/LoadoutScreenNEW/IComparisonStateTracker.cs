using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public interface IComparisonStateTracker
    {
        TargetType? SelectedItemType { get; }

        event EventHandler SelectedItemTypeChanged;
    }
}