using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public enum ItemType
    {
        Cruiser, Building, Unit
    }

    public interface IItem<TItem> : IStatefulUIElement, IGameObject
        where TItem : IComparableItem
	{
        ItemType Type { get; }
		TItem Item { get; }
		Image BackgroundImage { get; }
        bool ShowSelectedFeedback { set; }

        void SelectItem();
	}
}
