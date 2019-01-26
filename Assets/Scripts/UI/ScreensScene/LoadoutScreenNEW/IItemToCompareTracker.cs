using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    public interface IItemToCompareTracker
    {
        TargetType? ItemTypeToCompare { get; set; }

        event EventHandler ItemTypeToCompareChanged;
    }
}