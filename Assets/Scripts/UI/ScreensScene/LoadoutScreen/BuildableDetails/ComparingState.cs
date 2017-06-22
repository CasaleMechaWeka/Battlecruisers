using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails
{
	public class ComparingState<TItem> : BaseState<TItem> where TItem : IComparableItem
	{
		public ComparingState(ItemDetailsManager<TItem> itemDetailsManager)
			: base(itemDetailsManager) { }
	}
}

