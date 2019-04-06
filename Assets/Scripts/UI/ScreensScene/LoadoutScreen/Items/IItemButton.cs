using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Items
{
    public interface IItemButton
    {
        bool IsUnlocked { get; }
        IComparableItem Item { get; }
        Color Color { set; }
    }
}