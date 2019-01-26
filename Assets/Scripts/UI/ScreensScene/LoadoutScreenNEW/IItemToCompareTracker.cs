using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public interface IItemToCompareTracker
    {
        TargetType? ItemToCompare { get; }

        event EventHandler ItemToCompareChanged;
    }
}