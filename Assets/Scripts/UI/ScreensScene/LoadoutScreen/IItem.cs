using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
	// FELIX  Maybe create Item base class shared by Loadout- and Unlocked- Items?
	public interface IItem<TItem> where TItem : IComparableItem
	{
		TItem Item { get; }
		bool ShowSelectedFeedback { set; }
	}
}

