using BattleCruisers.Buildables;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.LoadoutItems;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails
{
	public class ComparingState<TItem> : BaseState<TItem> where TItem : IComparableItem
	{
		public ComparingState(IItemDetailsManager<TItem> itemDetailsManager)
			: base(itemDetailsManager) { }
	}
}

