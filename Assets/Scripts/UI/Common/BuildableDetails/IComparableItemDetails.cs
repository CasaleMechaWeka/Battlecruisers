using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using UnityEditor.Build;

namespace BattleCruisers.UI.Common.BuildableDetails
{
    public interface IComparableItemDetails<TItem> : IDismissableEmitter where TItem : IComparableItem
	{
		void ShowItemDetails(TItem item, TItem itemToCompareTo = default);
		void ShowItemDetails();
		void Hide();
		void SetHullType(HullType hullType);
		void SetBuilding(IBuilding building);
		void SetBuilding(IBuilding building, ItemButton button);
		void SetUnit(IUnit unit);
		void SetUnit(IUnit unit, ItemButton button);
	}
}
