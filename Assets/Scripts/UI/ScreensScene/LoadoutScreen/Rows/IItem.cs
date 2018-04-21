using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows
{
    public interface IItem<TItem> : IStatefulUIElement
        where TItem : IComparableItem
	{
		TItem Item { get; }
		Image BackgroundImage { get; }
        bool ShowSelectedFeedback { set; }
        void SelectItem();
	}
}
