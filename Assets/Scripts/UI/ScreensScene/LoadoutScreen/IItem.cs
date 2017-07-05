using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	public interface IItem<TItem> where TItem : IComparableItem
	{
		TItem Item { get; }
		bool ShowSelectedFeedback { set; }
	}
}

